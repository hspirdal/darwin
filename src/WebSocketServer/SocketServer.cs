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
    Task<bool> CheckValidConnectionAsync(string connectionId, Guid sessionId, IClientProxy clientProxy);
    void Remove(string connectionId);
    Task HandleClientMessageAsync(string connectionId, ClientRequest clientRequest);
  }

  public interface ISocketServer
  {
    Task SendAsync(string connectionId, ServerResponse response);
    List<Connection> ActiveConnections { get; }
  }

  public class SocketServer : ISocketServer, IClientRegistry
  {
    private readonly ILogger _logger;
    private readonly IStateRequestRouter _stateRequestRouter;
    private readonly IConnectionStore _connectionStore;

    public SocketServer(ILogger logger, IStateRequestRouter stateRequestRouter, IConnectionStore connectionStore)
    {
      _logger = logger;
      _stateRequestRouter = stateRequestRouter;
      _connectionStore = connectionStore;
    }

    public List<Connection> ActiveConnections => _connectionStore.ActiveConnections;

    public Task SendAsync(string connectionId, ServerResponse response)
    {
      try
      {
        var connection = _connectionStore.GetById(connectionId);
        if (connection != null)
        {
          var json = JsonConvert.SerializeObject(response);
          return connection.Client.SendAsync("direct", json);
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
      try
      {
        var success = await _connectionStore.ValidateConnectionAsync(connectionId, sessionId, clientProxy).ConfigureAwait(false);
        return success;
      }
      catch (Exception e)
      {
        _logger.Debug(e.Message);
        _logger.Error(e);
      }


      _logger.Debug("Not valid connection");
      return false;
    }

    public async Task HandleClientMessageAsync(string connectionId, ClientRequest clientRequest)
    {
      try
      {
        var connection = _connectionStore.GetById(clientRequest.SessionId);
        if (connection != null)
        {
          var clientId = connection.Id;
          await _stateRequestRouter.RouteAsync(clientId, clientRequest).ConfigureAwait(false);
        }
        else
        {
          _logger.Warn($"{nameof(HandleClientMessageAsync)}: connecton was null");
        }
      }
      catch (Exception e)
      {
        _logger.Error(e);
      }
    }

    public void Remove(string connectionId)
    {
      _connectionStore.Remove(connectionId);
    }
  }
}