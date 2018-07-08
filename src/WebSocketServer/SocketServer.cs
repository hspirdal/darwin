using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Actions;
using GameLib.Identities;
using GameLib.Logging;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TcpGameServer.Contracts;

namespace WebSocketServer
{
  public interface IClientRegistry
  {
    Guid GetSessionId(string connectionId);
    void Remove(string connectionId);
    Task<bool> CheckValidConnectionAsync(string connectionId, Guid sessionId, IClientProxy clientProxy);

    Task HandleClientMessageAsync(string connectionId, ClientRequest clientRequest);
  }

  public interface ISocketServer
  {
    Task SendAsync(string connectionId, ServerResponse response);
    List<Connection> GetConnections();
  }

  public class SocketServer : ISocketServer, IClientRegistry
  {
    private readonly ConcurrentDictionary<string, Connection> _connectionMap;
    private readonly ConcurrentDictionary<Guid, Connection> _sessionMap;
    private readonly ILogger _logger;
    private readonly IAuthenticator _authenticator;
    private readonly IStateRequestRouter _stateRequestRouter;
    private readonly IIdentityRepository _identityRepository;

    public SocketServer(ILogger logger, IAuthenticator authenticator, IStateRequestRouter stateRequestRouter, IIdentityRepository identityRepository)
    {
      _logger = logger;
      _authenticator = authenticator;
      _stateRequestRouter = stateRequestRouter;
      _identityRepository = identityRepository;

      _connectionMap = new ConcurrentDictionary<string, Connection>();
      _sessionMap = new ConcurrentDictionary<Guid, Connection>();
    }

    public Guid GetSessionId(string connectionId)
    {
      return _connectionMap[connectionId].SessionId;
    }

    public void Remove(string connectionId)
    {
      _logger.Debug($"Trying to remove connectionId '{connectionId}'");
      if (_connectionMap.ContainsKey(connectionId))
      {
        var success = _connectionMap.TryRemove(connectionId, out Connection connection);
        var msg = success ? "Successfully removed connection id" : "Was not able to remove connection id";
        _logger.Debug(msg);
      }
      else
      {
        _logger.Debug($"Could not find connectionId '{connectionId}'");
      }
    }

    public Task SendAsync(string connectionId, ServerResponse response)
    {
      try
      {
        if (_connectionMap.ContainsKey(connectionId))
        {
          var json = JsonConvert.SerializeObject(response);
          return _connectionMap[connectionId].Client.SendAsync("direct", json);
        }
        // NOTE: Race condition when connection dropped while server is sending out status messages.
        _logger.Warn($"Connection-id '{connectionId}' was not found.");

      }
      catch (Exception e)
      {
        _logger.Error(e);
      }
      return Task.CompletedTask;
    }

    public async Task<bool> CheckValidConnectionAsync(string connectionId, Guid sessionId, IClientProxy clientProxy)
    {
      _logger.Debug($"Validating connection - connectionId: '{connectionId}'. sessionId: '{sessionId}'");
      if (_connectionMap.ContainsKey(connectionId))
      {
        _logger.Debug("Found match against existing connection id: Check OK");
        return true;
      }

      if (_sessionMap.ContainsKey(sessionId))
      {
        _logger.Debug("Found match against existing session id: Check OK");
        _logger.Info("Refreshing connection map based on previous session");
        var connection = _sessionMap[sessionId];
        connection.ConnectionId = connectionId;
        connection.Client = clientProxy;
        if (RegisterNewConnection(connection))
        {
          return true;
        }
        _logger.Debug("Was not able to register connection..");
      }

      _logger.Debug("Connection id or session not saved previously. Looking up indentity based on stored session.");

      try
      {
        // TODO: extract out
        var identities = await _identityRepository.GetAllAsync().ConfigureAwait(false);
        _logger.Debug($"Found {identities.Count} identities in repository");
        var identity = identities.SingleOrDefault(i => i.SessionId == sessionId);
        if (identity != null)
        {
          var connection = new Connection
          {
            Id = identity.Id,
            ConnectionId = connectionId,
            SessionId = identity.SessionId,
            Client = clientProxy
          };
          _logger.Debug("Found session match in DB, registering connection and session in server memory.");
          if (RegisterNewConnection(connection) && RegisterNewSession(connection))
          {
            return true;
          }
        }
      }
      catch (Exception e)
      {
        _logger.Debug(e.Message);
        _logger.Error(e);
      }


      _logger.Debug("Not valid connection");
      return false;
    }

    public List<Connection> GetConnections()
    {
      return _connectionMap.Values.ToList();
    }

    public async Task HandleClientMessageAsync(string connectionId, ClientRequest clientRequest)
    {
      try
      {
        var clientId = GetClientId(connectionId);
        await _stateRequestRouter.RouteAsync(clientId, clientRequest).ConfigureAwait(false);
      }
      catch (Exception e)
      {
        _logger.Error(e);
      }
    }

    private bool RegisterNewConnection(Connection connection)
    {
      var success = _connectionMap.TryAdd(connection.ConnectionId, connection);
      if (!success)
      {
        _logger.Error(new InvalidOperationException($"Was not able to add connection with id {connection.Id} to connection map."));
        return false;
      }
      _logger.Debug($"Added connection id '{connection.ConnectionId} to connection map");
      return true;
    }

    private bool RegisterNewSession(Connection connection)
    {
      var success = _sessionMap.TryAdd(connection.SessionId, connection);
      if (!success)
      {
        _logger.Error(new InvalidOperationException($"Was not able to add session for connection id {connection.Id} to session map."));
        return false;
      }
      _logger.Debug($"Added session id '{connection.SessionId} to session map");
      return true;
    }

    private string GetClientId(string connectionId)
    {
      return _connectionMap[connectionId].Id;
    }
  }
}