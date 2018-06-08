using System;
using Microsoft.Extensions.Configuration;
using Ether.Network.Packets;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace TcpGameClient
{
	internal class Program
	{
		static void Main()
		{
			var client = new GameClient("127.0.0.1", 4444, 512, 5000);
			client.Connect();

			if (!client.IsConnected)
			{
				Console.WriteLine("Can't connect to server!");
				Console.ReadLine();
				return;
			}

			Console.WriteLine("Enter a message and press enter...");

			try
			{
				while (true)
				{
					var input = Console.ReadLine();

					if (input == "quit")
					{
						break;
					}

					if (input == "w")
					{
						using (var packet = new NetPacket())
						{
							var action = new MovementAction { OwnerId = 1, Name = "Action.Movement", MovementDirection = MovementDirection.West };
							var json = JsonConvert.SerializeObject(action);
							packet.Write(json);
							client.Send(packet);
						}
					}

					// if (input != null)
					// {
					// 	using (var packet = new NetPacket())
					// 	{
					// 		packet.Write(input);
					// 		client.Send(packet);
					// 	}
					// }
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}

			client.Disconnect();

			Console.WriteLine("Disconnected. Press any key to close the application...");
			Console.ReadLine();
		}
	}
}
