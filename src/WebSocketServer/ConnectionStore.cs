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
    void RegisterNewConnection(Connection connection);
    void Remove(string connectionId);
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

    public void RegisterNewConnection(Connection connection)
    {
      if (!_connectionMap.ContainsKey(connection.ConnectionId))
      {
        var connectionMapUpdated = _connectionMap.AddOrUpdate(connection.ConnectionId, connection, (string key, Connection Connection) =>
        {
          return connection;
        });
      }
      if (!_sessionMap.ContainsKey(connection.SessionId))
      {
        var sessionMapUpdated = _sessionMap.AddOrUpdate(connection.SessionId, connection, (Guid key, Connection Connection) =>
        {
          return connection;
        });
      }
    }

    public void Remove(string connectionId)
    {
      if (_connectionMap.ContainsKey(connectionId))
      {
        var success = _connectionMap.TryRemove(connectionId, out Connection connection);
        if (!success)
        {
          _logger.Warn($"Was not able to remove connection with connectionId '{connectionId}'");
        }
      }
    }

    public async Task<bool> ValidateConnectionAsync(string connectionId, Guid sessionId, IClientProxy clientProxy)
    {
      if (string.IsNullOrEmpty(connectionId) || sessionId == Guid.Empty)
      {
        return false;
      }

      if (_connectionMap.ContainsKey(connectionId) || _sessionMap.ContainsKey(sessionId))
      {
        return true;
      }

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

        RegisterNewConnection(newConnection);
        return true;
      }

      return false;
    }
  }
}