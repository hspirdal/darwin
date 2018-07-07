using System;
using System.Collections.Generic;
using Autofac;
using StackExchange.Redis;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Players;
using GameLib.Logging;
using GameLib.Identities;
using GameLib.Area;

namespace WebSocketServer
{
    public class Builder
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            RegisterRedis(builder);
            RegisterGameRouter(builder);
            RegisterActionResolver(builder);
            RegisterPlayArea(builder);
            RegisterGameConfigurations(builder);

            builder.RegisterType<SocketServer>().As<ISocketServer>().As<IClientRegistry>().SingleInstance();
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<IdentityRepository>().As<IIdentityRepository>().SingleInstance();
            builder.RegisterType<ActionRepository>().As<IActionRepository>().SingleInstance();
            builder.RegisterType<PlayerRepository>().As<IPlayerRepository>().SingleInstance();
            builder.RegisterType<PositionRepository>().As<IPositionRepository>().SingleInstance();
            builder.RegisterType<GameStateRepository>().As<IGameStateRepository>().SingleInstance();
            builder.RegisterType<Authenticator>().As<IAuthenticator>();
            builder.RegisterType<MapGenerator>().As<IMapGenerator>();
            builder.RegisterType<StartupTaskRunner>().As<IStartupTaskRunner>();
            builder.RegisterType<GameServer>().As<IGameServer>();
            builder.RegisterType<LobbyRouter>().As<ILobbyRouter>();
            builder.RegisterType<StateRequestRouter>().As<IStateRequestRouter>();

            return builder.Build();
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

        private static void RegisterGameRouter(ContainerBuilder builder)
        {
            builder.Register<GameRouter>(c =>
            {
                var movementInserter = new MovementInserter(c.Resolve<IActionRepository>());
                var inserterMap = new Dictionary<string, IRequestInserter>
                {
                    { movementInserter.ActionName, movementInserter }
                };

                return new GameRouter(c.Resolve<ILogger>(), inserterMap);
            }).As<IGameRouter>();
        }

        private static void RegisterActionResolver(ContainerBuilder builder)
        {
            builder.Register<ActionResolver>(c =>
            {
                var movementResolver = new MovementResolver(c.Resolve<IPositionRepository>(), c.Resolve<PlayArea>());
                var resolverMap = new Dictionary<string, IResolver>
                {
                    { movementResolver.ActionName, movementResolver }
                };

                return new ActionResolver(c.Resolve<IActionRepository>(), resolverMap);
            }).As<IActionResolver>();
        }

        private static void RegisterPlayArea(ContainerBuilder builder)
        {
            builder.Register<PlayArea>(c =>
            {
                var mapWidth = int.Parse(Environment.GetEnvironmentVariable("MapWidth"));
                var mapHeight = int.Parse(Environment.GetEnvironmentVariable("MapHeight"));
                var mapGenerator = c.Resolve<IMapGenerator>();
                return new PlayArea() { GameMap = mapGenerator.Generate(mapWidth, mapHeight) };
            }).SingleInstance();
        }

        private static void RegisterGameConfigurations(ContainerBuilder builder)
        {
            builder.Register<GameConfiguration>(c =>
            {
                var gameTickMiliseconds = int.Parse(Environment.GetEnvironmentVariable("GameTickMiliseconds"));
                return new GameConfiguration() { GameTickMiliseconds = gameTickMiliseconds };
            });
        }
    }
}