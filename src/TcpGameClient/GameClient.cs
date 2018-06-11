using Ether.Network.Client;
using Ether.Network.Packets;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using TcpGameServer.Contracts.Area;

namespace TcpGameClient
{
	public class GameClient : NetClient
	{
		public GameClient(string host, int port, int bufferSize, int timeOut)
		{
			Configuration.Host = host;
			Configuration.Port = port;
			Configuration.BufferSize = bufferSize;
			//Configuration.TimeOut = timeOut;
		}

		public override void HandleMessage(INetPacketStream packet)
		{
			var json = packet.Read<string>();
			var status = JsonConvert.DeserializeObject<StatusResponse>(json);

			Console.SetCursorPosition(0, 0);

			if (status != null)
			{
				for (var y = 0; y < status.Map.Height; ++y)
				{
					for (var x = 0; x < status.Map.Width; ++x)
					{
						var cell = status.Map.GetCell(x, y);
						var cellSymbol = cell.IsWalkable ? ' ' : '#';
						if (status.X == x && status.Y == y)
						{
							cellSymbol = '@';
						}

						Console.Write(cellSymbol);
					}
					Console.Write(Environment.NewLine);
				}
			}
		}

		protected override void OnConnected()
		{
			Console.WriteLine("Connected to {0}", this.Socket.RemoteEndPoint);
		}

		protected override void OnDisconnected()
		{
			Console.WriteLine("Disconnected");
		}

		protected override void OnSocketError(SocketError socketError)
		{
			Console.WriteLine("Socket Error: {0}", socketError.ToString());
		}
	}
}