using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Logging;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using GameLib.Identities;

namespace WebSocketServer
{
  public interface IConnectionStore
  {
    Task<bool> ValidateConnectionAsync(string connectionId, Guid sessionId, IClientProxy clientProxy);
    Connection GetById(string connectionId);
    Connection GetById(Guid sessionId);
    List<Connection> ActiveConnections { get; }
    void Remove(string connectionId);
    void RegisterNewConnection(Connection connection);
  }

  public class ConnectionStore : IConnectionStore
  {
    private readonly ConcurrentDictionary<string, Connection> _connectionMap;
    private readonly ConcurrentDictionary<Guid, Connection> _sessionMap;
    private readonly ILogger _logger;
    private readonly IIdentityRepository _identityRepository;

    public ConnectionStore(ILogger logger, IIdentityRepository identityRepository)
    {
      _logger = logger;
      _identityRepository = identityRepository;

      _sessionMap = new ConcurrentDictionary<Guid, Connection>();
      _connectionMap = new ConcurrentDictionary<string, Connection>();
    }

    public Connection GetById(string connectionId)
    {
      if (_connectionMap.ContainsKey(connectionId))
      {
        var success = _connectionMap.TryGetValue(connectionId, out Connection connection);
        if (success)
        {
          return connection;
        }
        _logger.Warn($"Could not retrieve connection by connectionId '{connectionId}' at this time");
      }
      return null;
    }

    public Connection GetById(Guid sessionId)
    {
      if (_sessionMap.ContainsKey(sessionId))
      {
        var success = _sessionMap.TryGetValue(sessionId, out Connection connection);
        if (success)
        {
          return connection;
        }
        _logger.Warn($"Could not retrieve connection by sessionId '{sessionId}' at this time");
      }
      return null;
    }

    public List<Connection> ActiveConnections => _connectionMap.Values.ToList();

    public async Task<bool> ValidateConnectionAsync(string connectionId, Guid sessionId, IClientProxy clientProxy)
    {
      _logger.Debug($"Validating connection - connectionId: '{connectionId}'. sessionId: '{sessionId}'");

      if (string.IsNullOrEmpty(connectionId) || sessionId == Guid.Empty)
      {
        _logger.Warn($"Empty params - {connectionId} // {sessionId}");
        return false;
      }

      if (_connectionMap.ContainsKey(connectionId))
      {
        return true;
      }

      if (_sessionMap.ContainsKey(sessionId))
      {
        UpdateConnectionIdForClient(sessionId, connectionId, clientProxy);
        return true;
      }

      _logger.Debug("Connection id or session not saved previously. Looking up indentity based on stored session.");

      var identities = await _identityRepository.GetAllAsync().ConfigureAwait(false);
      var identity = identities.SingleOrDefault(i => i.SessionId == sessionId);
      if (identity != null)
      {
        Connection newConnection = new Connection
        {
          Id = identity.Id,
          ConnectionId = connectionId,
          SessionId = sessionId,
          Client = clientProxy
        };

        _logger.Debug("Found session match in DB, registering connection and session in server memory.");
        RegisterNewConnection(newConnection);
        return true;
      }

      _logger.Warn($"Failed to authenticate connection with id '{connectionId}'");
      return false;
    }

    public void Remove(string connectionId)
    {
      if (_connectionMap.ContainsKey(connectionId))
      {
        var success = _connectionMap.TryRemove(connectionId, out Connection connection);
        _logger.Debug($"Remove connection id '{connectionId}' - success: {success}");
      }
    }

    public void RegisterNewConnection(Connection connection)
    {
      if (!_connectionMap.ContainsKey(connection.ConnectionId))
      {
        var connectionMapUpdated = _connectionMap.AddOrUpdate(connection.ConnectionId, connection, (string key, Connection Connection) =>
        {
          return connection;
        });
        _logger.Debug($"Added connection id '{connection.ConnectionId} to connection map");
      }
      if (!_sessionMap.ContainsKey(connection.SessionId))
      {
        var sessionMapUpdated = _sessionMap.AddOrUpdate(connection.SessionId, connection, (Guid key, Connection Connection) =>
        {
          return connection;
        });
        _logger.Debug($"Added session id '{connection.SessionId} to session map");
      }
    }

    private void UpdateConnectionIdForClient(Guid sessionId, string newConnectionId, IClientProxy clientProxy)
    {
      var connection = GetById(sessionId);
      var staleConnectionId = connection.ConnectionId;
      connection.ConnectionId = newConnectionId;
      connection.Client = clientProxy;
      _logger.Debug($"Updating a stale connection id. Session id: '{sessionId}'. Stale connection id: '{staleConnectionId}'");
      Remove(staleConnectionId);
      RegisterNewConnection(connection);
    }
  }
}