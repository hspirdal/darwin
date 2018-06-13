using System;
using Autofac;
using StackExchange.Redis;
using TcpGameServer.Actions;
using TcpGameServer.Actions.Movement;
using TcpGameServer.Players;
using TcpGameServer.Logging;
using TcpGameServer.Identities;
using System.Collections.Generic;
using TcpGameServer.Area;

namespace TcpGameServer
{
	public class Builder
	{
		public static IContainer CreateContainer()
		{
			var builder = new ContainerBuilder();

			RegisterRedis(builder);
			RegisterTcpServer(builder);
			RegisterRequestRouter(builder);
			RegisterPlayArea(builder);

			builder.RegisterType<Logger>().As<ILogger>();
			builder.RegisterType<IdentityRepository>().As<IIdentityRepository>().SingleInstance();
			builder.RegisterType<ActionRepository>().As<IActionRepository>().SingleInstance();
			builder.RegisterType<PlayerRepository>().As<IPlayerRepository>().SingleInstance();
			builder.RegisterType<PositionRepository>().As<IPositionRepository>().SingleInstance();
			builder.RegisterType<MovementResolver>().As<IMovementResolver>();
			builder.RegisterType<ActionResolver>().As<IActionResolver>();
			builder.RegisterType<Authenticator>().As<IAuthenticator>();
			builder.RegisterType<MapGenerator>().As<IMapGenerator>();
			builder.RegisterType<StartupTaskRunner>().As<IStartupTaskRunner>();
			builder.RegisterType<GameServer>().As<IGameServer>();

			return builder.Build();
		}

		private static void RegisterTcpServer(ContainerBuilder builder)
		{
			builder.Register<TcpServer>(c =>
			{
				var host = Environment.GetEnvironmentVariable("TcpGameServerHost");
				if (string.IsNullOrEmpty(host))
				{
					throw new ArgumentException("Host was not defined as environment variable.");
				}

				var server = new TcpServer(c.Resolve<ILogger>(), c.Resolve<IAuthenticator>(), c.Resolve<IRequestRouter>(), host);
				return server;
			}).As<TcpServer>().As<ITcpServer>().SingleInstance();
		}

		private static void RegisterRedis(ContainerBuilder builder)
		{
			builder.Register<ConnectionMultiplexer>(c =>
			{
				var configuration = new ConfigurationOptions { ResolveDns = true, SyncTimeout = 5000 };
				configuration.EndPoints.Add(Environment.GetEnvironmentVariable("RedisHost"));
				var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
				return connectionMultiplexer;
			}).As<IConnectionMultiplexer>().SingleInstance();
		}

		private static void RegisterRequestRouter(ContainerBuilder builder)
		{
			builder.Register<RequestRouter>(c =>
			{
				var movementInserter = new MovementInserter(c.Resolve<IActionRepository>());
				var inserterMap = new Dictionary<string, IRequestInserter>
				{
					{ movementInserter.ActionName, movementInserter }
				};

				return new RequestRouter(c.Resolve<ILogger>(), inserterMap);
			}).As<IRequestRouter>();
		}

		private static void RegisterPlayArea(ContainerBuilder builder)
		{
			builder.Register<PlayArea>(c =>
			{
				var mapGenerator = c.Resolve<IMapGenerator>();
				return new PlayArea() { Map = mapGenerator.Generate(30, 15) };
			}).SingleInstance();
		}
	}
}