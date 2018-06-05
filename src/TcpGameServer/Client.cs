using Ether.Network.Common;
using Ether.Network.Packets;
using System;
using System.Threading.Tasks;

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

			Console.WriteLine($"Received '{value}' from {Id}");

			using (var p = new NetPacket())
			{
				p.Write(string.Format($"OK: '{value}'"));
				Server.SendToAll(p);
			}
		}
	}
}