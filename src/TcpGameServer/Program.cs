using System;
using System.Threading.Tasks;
using System.Threading;
using Ether.Network.Packets;

namespace TcpGameServer
{
	internal class Program
	{
		private static async Task Main()
		{
			var host = Environment.GetEnvironmentVariable("TcpGameServerHost");
			if (string.IsNullOrEmpty(host))
			{
				Console.WriteLine("Host was not defined as environment variable.");
				return;
			}

			var server = new TcpServer(host);
			new Thread(() =>
			{
				server.Start();
			}).Start();

			while (true)
			{
				await Task.Delay(1000);
				await server.BroadcastAsync("Resolving all actions...");
			}
		}
	}
}
