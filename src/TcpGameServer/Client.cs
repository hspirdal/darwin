using Ether.Network.Common;
using Ether.Network.Packets;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TcpGameServer.Actions.Movement;

namespace TcpGameServer
{
	public class Client : NetUser
	{
		public void SendWelcomeMessage()
		{
			using (var p = new NetPacket())
			{
				p.Write($"Your connection id is '{Id}'");
				Send(p);
			}
		}

		public override void HandleMessage(INetPacketStream packet)
		{
			var value = packet.Read<string>();

			var movementAction = JsonConvert.DeserializeObject<MovementAction>(value);

			((TcpServer)Server).TempResolveAction(movementAction);

			Console.WriteLine($"Client wants to move: {movementAction.MovementDirection.ToString()}");


			// Console.WriteLine($"Received '{value}' from {Id}");

			// using (var p = new NetPacket())
			// {
			// 	p.Write(string.Format($"OK: '{value}'"));
			// 	Server.SendToAll(p);
			// }
		}
	}
}