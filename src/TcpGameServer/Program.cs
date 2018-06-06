using System;
using System.Threading.Tasks;
using System.Threading;
using Ether.Network.Packets;
using StackExchange.Redis;
using Autofac;
using TcpGameServer.Actions;
using TcpGameServer.Actions.Movement;
using TcpGameServer.identity;

namespace TcpGameServer
{
	internal class Program
	{
		private static async Task Main()
		{
			var container = Builder.CreateContainer();
			using (var scope = container.BeginLifetimeScope())
			{
				var startupTaskRunner = scope.Resolve<IStartupTaskRunner>();
				await startupTaskRunner.ExecuteAsync().ConfigureAwait(false);

				var server = scope.Resolve<TcpServer>();

				new Thread(() =>
				{
					server.Start();
				}).Start();

				var gameServer = scope.Resolve<IGameServer>();
				await gameServer.StartAsync().ConfigureAwait(false);
			}
		}
	}
}
