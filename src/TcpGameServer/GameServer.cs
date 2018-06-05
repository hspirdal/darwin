using Ether.Network.Server;
using System;

namespace TcpGameServer
{
	public class GameServer : NetServer<Client>
	{
		public GameServer()
		{
			Configuration.Backlog = 100;
			Configuration.Port = 4445;
			Configuration.MaximumNumberOfConnections = 100;
			Configuration.Host = "127.0.0.1";
			Configuration.BufferSize = 8;
			Configuration.Blocking = true;
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
		}

		protected override void OnError(Exception exception)
		{
			// TBA
		}
	}
}