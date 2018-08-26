using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autofac;
using GameLib.Identities;
using GameLib;

namespace WebSocketServer
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var container = Builder.CreateContainer();
			using (var scope = container.BeginLifetimeScope())
			{
				var clientRegistry = scope.Resolve<IClientRegistry>();
				var authenticator = scope.Resolve<IAuthenticator>();
				var host = CreateWebHostBuilder(args, clientRegistry, authenticator).Build();
				host.Start();

				var startupTaskRunner = scope.Resolve<IStartupTaskRunner>();
				await startupTaskRunner.ExecuteAsync().ConfigureAwait(false);

				var gameServer = scope.Resolve<IGameServer>();
				Console.WriteLine("Starting game server..");
				await gameServer.StartAsync().ConfigureAwait(false);
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args, IClientRegistry clientRegistry, IAuthenticator authenticator) =>
				WebHost.CreateDefaultBuilder(args)
						.ConfigureServices(services =>
						{
							services.AddSingleton<IClientRegistry>(clientRegistry);
							services.AddSingleton<IAuthenticator>(authenticator);
							services.AddSingleton<ConcurrentRegistry<string, string>>();
						})
						.UseStartup<Startup>();
	}
}
