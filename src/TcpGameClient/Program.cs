using System;
using Microsoft.Extensions.Configuration;
using Ether.Network.Packets;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using TcpGameServer.Contracts;

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
				var authRequest = new ClientRequest
				{
					RequestName = "Authenticate",
					Payload = "arch;1234"
				};

				SendRequest<ClientRequest>(client, authRequest);



				while (true)
				{
					var input = Console.ReadLine();

					if (input == "quit")
					{
						break;
					}

					if (input == "w")
					{
						var action = new MovementAction { OwnerId = 1, Name = "Action.Movement", MovementDirection = MovementDirection.West };
						var request = new ClientRequest
						{
							RequestName = "Action.Movement",
							Payload = JsonConvert.SerializeObject(action)
						};
						SendRequest(client, request);
					}
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

		private static void SendRequest<T>(GameClient client, T request)
		{
			using (var packet = new NetPacket())
			{
				var json = JsonConvert.SerializeObject(request);
				packet.Write(json);
				client.Send(packet);
			}
		}
	}
}
