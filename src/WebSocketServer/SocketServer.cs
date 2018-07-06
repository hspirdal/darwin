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
        void Remove(string connectionId);
        bool CheckValidConnection(string connectionId, Guid sessionId);

        Task<bool> AuthenticateAsync(AuthentificationRequest request, IClientProxy clientProxy);
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

        public SocketServer(ILogger logger, IAuthenticator authenticator, IStateRequestRouter stateRequestRouter)
        {
            _logger = logger;
            _authenticator = authenticator;
            _stateRequestRouter = stateRequestRouter;

            _connectionMap = new ConcurrentDictionary<string, Connection>();
            _sessionMap = new ConcurrentDictionary<Guid, Connection>();
        }

        public void Remove(string connectionId)
        {
            if (_connectionMap.ContainsKey(connectionId))
            {
                _connectionMap.TryRemove(connectionId, out Connection connection);
            }
        }

        public Task SendAsync(string connectionId, ServerResponse response)
        {
            if (_connectionMap.ContainsKey(connectionId))
            {
                var json = JsonConvert.SerializeObject(response);
                return _connectionMap[connectionId].Client.SendAsync("direct", json);
            }

            throw new ArgumentException($"Connection-id '{connectionId}' was not found.");
        }

        public bool CheckValidConnection(string connectionId, Guid sessionId)
        {
            if (_connectionMap.ContainsKey(connectionId))
            {
                return true;
            }

            if (_sessionMap.ContainsKey(sessionId))
            {
                _logger.Info("Refreshing connection map based on previous session");
                var connection = _sessionMap[sessionId];
                if (RegisterNewConnection(connection))
                {
                    connection.ConnectionId = connectionId;
                    return true;
                }
            }

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

        public async Task<bool> AuthenticateAsync(AuthentificationRequest request, IClientProxy clientProxy)
        {
            var identity = await _authenticator.AuthenticateAsync(request).ConfigureAwait(false);
            if (identity != null)
            {
                var connection = new Connection
                {
                    Id = identity.Id,
                    ConnectionId = request.ConnectionId,
                    SessionId = Guid.NewGuid(),
                    Client = clientProxy
                };

                return RegisterNewConnection(connection) && RegisterNewSession(connection);
            }
            return false;
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