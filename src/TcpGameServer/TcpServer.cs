using Ether.Network.Packets;
using Ether.Network.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TcpGameServer.Actions;
using TcpGameServer.Contracts;
using TcpGameServer.Identities;
using TcpGameServer.Logging;

namespace TcpGameServer
{
	public interface ITcpServer
	{
		void Broadcast(string message);
		void TempSend(Guid connectionId, string message);
		List<Connection> GetConnections();
	}

	public class Connection
	{
		public string Id { get; set; }
		public Client Client { get; set; }
	}

	public class TcpServer : NetServer<Client>, ITcpServer
	{
		private readonly ConcurrentDictionary<Guid, Connection> _connectionMap;
		private readonly IAuthenticator _authenticator;
		private readonly ILogger _logger;
		private readonly IRequestRouter _requestRouter;

		public TcpServer(ILogger logger, IAuthenticator authenticator, IRequestRouter requestRouter, string host)
		{
			_logger = logger;
			_authenticator = authenticator;
			_requestRouter = requestRouter;

			Configuration.Backlog = 100;
			Configuration.Port = 4445;
			Configuration.MaximumNumberOfConnections = 100;
			Configuration.Host = host;
			Configuration.BufferSize = 8;
			Configuration.Blocking = true;

			_connectionMap = new ConcurrentDictionary<Guid, Connection>();
		}

		protected override void Initialize()
		{
			_logger.Info("Server is ready.");
		}

		protected override void OnClientConnected(Client connection)
		{
			_logger.Info("New client connected!");
			connection.SendWelcomeMessage();
		}

		protected override void OnClientDisconnected(Client connection)
		{
			_logger.Info("Client disconnected!");
			var successs = _connectionMap.TryRemove(connection.Id, out Connection c);
			if (!successs)
			{
				_logger.Warn($"Not able to remove connection with id {connection.Id}");
			}
		}

		protected override void OnError(Exception exception)
		{
			_logger.Error(exception);
		}

		public void Broadcast(string message)
		{
			var p = new NetPacket();
			p.Write(message);
			SendToAll(p);
		}

		public void TempSend(Guid connectionId, string message)
		{
			var p = new NetPacket();
			p.Write(message);
			SendTo(new List<Client>() { _connectionMap[connectionId].Client }, p);
		}

		public string GetClientId(Guid connectionId)
		{
			return _connectionMap[connectionId].Id;
		}

		public void RouteRequest(string clientId, ClientRequest clientRequest)
		{
			_requestRouter.Route(clientId, clientRequest);
		}

		public bool IsAuthenticated(Guid connectionId)
		{
			return _connectionMap.ContainsKey(connectionId);
		}

		public bool Authenticate(AuthentificationRequest request, Client client)
		{
			var identity = _authenticator.Authenticate(request);
			if (identity != null)
			{
				var success = _connectionMap.TryAdd(request.ConnectionId, new Connection
				{
					Id = identity.Id,
					Client = client
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

		public List<Connection> GetConnections()
		{
			return _connectionMap.Values.ToList();
		}
	}
}