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
        //void Add(string connectionId, IClientProxy clientProxy);
        void Remove(string connectionId);
        bool IsAuthenticated(string connectionId);
        bool Authenticate(AuthentificationRequest request, IClientProxy clientProxy);
        void HandleClientMessage(string connectionId, string data);
    }

    public class Connection
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public IClientProxy Client { get; set; }
    }

    public interface ISocketServer
    {
        Task SendAsync(string connectionId, string message);
        List<Connection> GetConnections();
    }

    public class SocketServer : ISocketServer, IClientRegistry
    {
        private readonly ConcurrentDictionary<string, Connection> _connectionMap;
        private readonly ILogger _logger;
        private readonly IAuthenticator _authenticator;
        private readonly IStateRequestRouter _stateRequestRouter;

        public SocketServer(ILogger logger, IAuthenticator authenticator, IStateRequestRouter stateRequestRouter)
        {
            _logger = logger;
            _authenticator = authenticator;
            _stateRequestRouter = stateRequestRouter;

            _connectionMap = new ConcurrentDictionary<string, Connection>();
        }

        public Task SendAsync(string connectionId, string message)
        {
            if (_connectionMap.ContainsKey(connectionId))
            {
                return _connectionMap[connectionId].Client.SendAsync("direct", message);
            }

            throw new ArgumentException($"Connection-id '{connectionId}' was not found.");
        }

        public void Remove(string connectionId)
        {
            if (_connectionMap.ContainsKey(connectionId))
            {
                _connectionMap.TryRemove(connectionId, out Connection connection);
            }
        }

        public bool IsAuthenticated(string connectionId)
        {
            return _connectionMap.ContainsKey(connectionId);
        }

        public string GetClientId(string connectionId)
        {
            return _connectionMap[connectionId].Id;
        }

        public List<Connection> GetConnections()
        {
            return _connectionMap.Values.ToList();
        }

        public void HandleClientMessage(string connectionId, string data)
        {
            try
            {
                if (!IsAuthenticated(connectionId))
                {
                    throw new ArgumentException("Client is not authorized");
                }

                var clientId = GetClientId(connectionId);
                var clientRequest = JsonConvert.DeserializeObject<ClientRequest>(data);
                if (clientRequest != null)
                {
                    RouteRequest(clientId, clientRequest);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void RouteRequest(string clientId, ClientRequest clientRequest)
        {
            _stateRequestRouter.Route(clientId, clientRequest);
        }

        public bool Authenticate(AuthentificationRequest request, IClientProxy clientProxy)
        {
            var identity = _authenticator.Authenticate(request);
            if (identity != null)
            {
                var success = _connectionMap.TryAdd(request.ConnectionId, new Connection
                {
                    Id = identity.Id,
                    ConnectionId = request.ConnectionId,
                    Client = clientProxy
                });

                if (!success)
                {
                    _logger.Error(new InvalidOperationException($"Was not able to add connection with id {identity.Id} to connection map."));
                    return false;
                }
                _logger.Info($"Successfully logged on client with id {identity.Id}");
                return true;
            }
            return false;
        }
    }
}