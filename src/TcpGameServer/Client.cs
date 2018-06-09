using Ether.Network.Common;
using Ether.Network.Packets;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TcpGameServer.Actions.Movement;
using TcpGameServer.Contracts;
using TcpGameServer.Identities;

namespace TcpGameServer
{
	public class Client : NetUser
	{
		private TcpServer _server;

		public void SendWelcomeMessage()
		{
			using (var p = new NetPacket())
			{
				p.Write($"Connection established. Expecting authentication.");
				Send(p);
			}
		}

		public override void HandleMessage(INetPacketStream packet)
		{
			var json = packet.Read<string>();
			try
			{
				if (Server.IsAuthenticated(Id))
				{
					Console.WriteLine("authed");
					var clientRequest = JsonConvert.DeserializeObject<ClientRequest>(json);
					var movementAction = JsonConvert.DeserializeObject<MovementAction>(clientRequest.Payload);
					Server.TempResolveAction(movementAction);
					Console.WriteLine($"Client wants to move: {movementAction.MovementDirection.ToString()}");
				}
				else
				{
					TempAuthenticate(json);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private void TempAuthenticate(string json)
		{
			Console.WriteLine("begin auth");
			var clientRequest = JsonConvert.DeserializeObject<ClientRequest>(json);
			var connectionString = clientRequest.Payload;
			var kvp = connectionString.Split(';');
			var request = new AuthentificationRequest
			{
				UserName = kvp[0],
				Password = kvp[1],
				ConnectionId = Id
			};

			Console.WriteLine("Trying to auth..");
			var success = Server.Authenticate(request, this);
			var msg = success ? "Welcome back" : "Could not authenticate";
			Console.WriteLine(msg);
			SendMessage(msg);
		}

		public new TcpServer Server
		{
			get
			{
				if (_server == null)
				{
					_server = (TcpServer)base.Server;
				}

				return _server;
			}
		}

		private void SendMessage(string message)
		{
			using (var p = new NetPacket())
			{
				p.Write(message);
				Send(p);
			}
		}
	}
}