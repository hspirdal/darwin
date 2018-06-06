using System;

namespace TcpGameServer
{
	internal class Program
	{
		private static void Main()
		{
			var host = Environment.GetEnvironmentVariable("TcpGameServerHost");
			if (string.IsNullOrEmpty(host))
			{
				Console.WriteLine("Host was not defined as environment variable.");
				return;
			}

			using (var server = new GameServer(host))
			{
				server.Start();
			}
		}
	}
}
