using System;

namespace TcpGameServer
{
	internal class Program
	{
		private static void Main()
		{
			using (var server = new GameServer())
			{
				server.Start();
			}
		}
	}
}
