using Ether.Network.Packets;
using Ether.Network.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TcpGameServer.Actions;

namespace TcpGameServer
{
	public interface ITcpServer
	{
		void Broadcast(string message);
	}

	public class TcpServer : NetServer<Client>, ITcpServer
	{
		private readonly Dictionary<Guid, Client> _clientMap;
		private readonly IActionRepository _actionRepository;

		public TcpServer(IActionRepository actionRepository, string host)
		{
			_actionRepository = actionRepository;

			Configuration.Backlog = 100;
			Configuration.Port = 4445;
			Configuration.MaximumNumberOfConnections = 100;
			Configuration.Host = host;
			Configuration.BufferSize = 8;
			Configuration.Blocking = true;

			_clientMap = new Dictionary<Guid, Client>();
		}

		protected override void Initialize()
		{
			Console.WriteLine("Server is ready.");
		}

		protected override void OnClientConnected(Client connection)
		{
			Console.WriteLine("New client connected!");
			_clientMap.Add(connection.Id, connection);

			connection.SendWelcomeMessage();
		}

		protected override void OnClientDisconnected(Client connection)
		{
			Console.WriteLine("Client disconnected!");
			_clientMap.Remove(connection.Id);
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
			_actionRepository.EnqueueAction(action);
		}
	}
}