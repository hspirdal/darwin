using Ether.Network.Packets;
using Ether.Network.Server;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TcpGameServer.Actions;
using TcpGameServer.Identities;
using TcpGameServer.Logging;

namespace TcpGameServer
{
	public interface ITcpServer
	{
		void Broadcast(string message);
	}

	public class Connection
	{
		public string Id { get; set; }
		public Client Client { get; set; }
	}

	public class TcpServer : NetServer<Client>, ITcpServer
	{
		private readonly ConcurrentDictionary<Guid, Connection> _connectionMap;
		private readonly IActionRepository _actionRepository;
		private readonly IAuthenticator _authenticator;
		private readonly ILogger _logger;

		public TcpServer(ILogger logger, IActionRepository actionRepository, IAuthenticator authenticator, string host)
		{
			_logger = logger;
			_authenticator = authenticator;
			_actionRepository = actionRepository;

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
			Console.WriteLine("Server is ready.");
		}

		protected override void OnClientConnected(Client connection)
		{
			Console.WriteLine("New client connected!");
			connection.SendWelcomeMessage();
		}

		protected override void OnClientDisconnected(Client connection)
		{
			Console.WriteLine("Client disconnected!");
			var successs = _connectionMap.TryRemove(connection.Id, out Connection c);
			if (!successs)
			{
				_logger.Warning($"Not able to remove connection with id {connection.Id}");
			}
		}

		protected override void OnError(Exception exception)
		{
			// TBA
		}

		public void Broadcast(string message)
		{
			var p = new NetPacket();
			p.Write(message);
			SendToAll(p);
		}

		public void TempResolveAction(Actions.Action action)
		{
			_actionRepository.PushInto(action);
		}

		public bool IsAuthenticated(Guid connectionId)
		{
			Console.WriteLine($"IsAuth: {connectionId}, isnull: {_connectionMap == null}");

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
	}
}