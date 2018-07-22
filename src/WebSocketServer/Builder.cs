using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using StackExchange.Redis;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Entities;
using GameLib.Properties;
using GameLib.Logging;
using GameLib.Identities;
using GameLib.Area;
using GameLib.Actions.Loot;
using GameLib.Properties.Stats;
using GameLib.Utility;

namespace WebSocketServer
{
	public class Builder
	{
		public static IContainer CreateContainer()
		{
			var builder = new ContainerBuilder();

			RegisterAutoMapper(builder);
			RegisterRedis(builder);
			RegisterGameRouter(builder);
			RegisterActionResolver(builder);
			RegisterCreatureFactory(builder);
			RegisterPlayArea(builder);
			RegisterGameConfigurations(builder);

			builder.RegisterType<SocketServer>().As<ISocketServer>().As<IClientRegistry>().SingleInstance();
			builder.RegisterType<Logger>().As<ILogger>();
			builder.RegisterType<IdentityRepository>().As<IIdentityRepository>().SingleInstance();
			builder.RegisterType<ActionRepository>().As<IActionRepository>().SingleInstance();
			builder.RegisterType<PlayerRepository>().As<IPlayerRepository>().SingleInstance();
			builder.RegisterType<ConnectionStore>().As<IConnectionStore>().SingleInstance();
			builder.RegisterType<Authenticator>().As<IAuthenticator>();
			builder.RegisterType<MapGenerator>().As<IMapGenerator>();
			builder.RegisterType<StartupTaskRunner>().As<IStartupTaskRunner>();
			builder.RegisterType<GameServer>().As<IGameServer>();
			builder.RegisterType<LobbyRouter>().As<ILobbyRouter>();
			builder.RegisterType<StateRequestRouter>().As<IStateRequestRouter>();
			builder.RegisterType<RandomGenerator>().As<IRandomGenerator>();
			builder.RegisterType<FeedbackRepository>().As<IFeedbackRepository>().As<IFeedbackWriter>().SingleInstance();

			return builder.Build();
		}

		private static void RegisterRedis(ContainerBuilder builder)
		{
			builder.Register<ConnectionMultiplexer>(c =>
			{
				var configuration = new ConfigurationOptions { ResolveDns = true, SyncTimeout = 5000 };
				configuration.EndPoints.Add(Environment.GetEnvironmentVariable("RedisHost"));
				var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
				connectionMultiplexer.PreserveAsyncOrder = false;
				return connectionMultiplexer;
			}).As<IConnectionMultiplexer>().SingleInstance();
		}

		private static void RegisterGameRouter(ContainerBuilder builder)
		{
			builder.Register<GameRouter>(c =>
			{
				var movementInserter = new MovementInserter(c.Resolve<ILogger>(), c.Resolve<IActionRepository>());
				var lootAllInserter = new LootAllInserter(c.Resolve<ILogger>(), c.Resolve<IActionRepository>());
				var lootInserter = new LootInserter(c.Resolve<ILogger>(), c.Resolve<IActionRepository>());
				var inserterMap = new Dictionary<string, IRequestInserter>
					{
						{ movementInserter.ActionName, movementInserter },
						{ lootAllInserter.ActionName, lootAllInserter},
						{ lootInserter.ActionName, lootInserter}
					};

				return new GameRouter(c.Resolve<ILogger>(), inserterMap);
			}).As<IGameRouter>();
		}

		private static void RegisterActionResolver(ContainerBuilder builder)
		{
			builder.Register<ActionResolver>(c =>
			{
				var movementResolver = new MovementResolver(c.Resolve<ILogger>(), c.Resolve<IFeedbackWriter>(), c.Resolve<IPlayerRepository>(), c.Resolve<IPlayArea>());
				var lootAllResolver = new LootAllResolver(c.Resolve<ILogger>(), c.Resolve<IFeedbackWriter>(), c.Resolve<IPlayerRepository>(), c.Resolve<IPlayArea>());
				var lootResolver = new LootResolver(c.Resolve<ILogger>(), c.Resolve<IFeedbackWriter>(), c.Resolve<IPlayerRepository>(), c.Resolve<IPlayArea>());
				var resolverMap = new Dictionary<string, IResolver>
					{
						{ movementResolver.ActionName, movementResolver },
						{ lootAllResolver.ActionName, lootAllResolver },
						{ lootResolver.ActionName, lootResolver }
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
				var playArea = new PlayArea() { GameMap = mapGenerator.Generate(mapWidth, mapHeight) };
				// Temp until there exists a better map initialization place.
				var itemSpawner = new ItemSpawner(playArea);
				var totalItemsToAdd = (int)((playArea.GameMap.Width * playArea.GameMap.Height) * 0.01);
				itemSpawner.AddRandomly(totalItemsToAdd);
				var creatureSpawner = new CreatureSpawner(playArea, c.Resolve<ICreatureFactory>());
				var totalCreaturesToAdd = (int)((playArea.GameMap.Width * playArea.GameMap.Height) * 0.01);
				creatureSpawner.SpawnRandomly(totalCreaturesToAdd);
				return playArea;
			}).As<IPlayArea>().SingleInstance();
		}

		private static void RegisterCreatureFactory(ContainerBuilder builder)
		{
			builder.Register<CreatureFactory>(c =>
			{
				var templates = new List<Creature>
				{
					new Creature(Guid.NewGuid().ToString(), "Orc Warrior", "Orc", "Fighter", new Statistics{
						AbilityScores = new AbilityScores(17, 11, 12, 7, 8, 6),
						AttackScores = new AttackScores(),
						DefenseScores = new DefenseScores(13, 6)
					}),
					new Creature(Guid.NewGuid().ToString(), "Dire Bat", "Animal", string.Empty, new Statistics{
						AbilityScores = new AbilityScores(17, 15, 13, 2, 14, 6),
						AttackScores = new AttackScores(),
						DefenseScores = new DefenseScores(14, 22)
					}),
					new Creature(Guid.NewGuid().ToString(), "Ghoul", "Undead", string.Empty, new Statistics{
						AbilityScores = new AbilityScores(13, 15, 0, 13, 14, 14),
						AttackScores = new AttackScores(),
						DefenseScores = new DefenseScores(14, 13)
					}),
					new Creature(Guid.NewGuid().ToString(), "Grizzly Bear", "Animal", string.Empty, new Statistics{
						AbilityScores = new AbilityScores(21, 13, 19, 2, 12, 6),
						AttackScores = new AttackScores(),
						DefenseScores = new DefenseScores(16, 42)
					}),
				};

				return new CreatureFactory(templates, c.Resolve<IRandomGenerator>());
			}).As<ICreatureFactory>();
		}

		private static void RegisterGameConfigurations(ContainerBuilder builder)
		{
			builder.Register<GameConfiguration>(c =>
			{
				var gameTickMiliseconds = int.Parse(Environment.GetEnvironmentVariable("GameTickMiliseconds"));
				return new GameConfiguration() { GameTickMiliseconds = gameTickMiliseconds };
			});
		}

		private static void RegisterAutoMapper(ContainerBuilder builder)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			builder.RegisterAssemblyTypes(assemblies)
					.Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
					.As<Profile>();

			builder.Register(c => new MapperConfiguration(cfg =>
			{
				foreach (var profile in c.Resolve<IEnumerable<Profile>>())
				{
					cfg.AddProfile(profile);
				}
			})).AsSelf().SingleInstance();

			builder.Register(c => c.Resolve<MapperConfiguration>()
					.CreateMapper(c.Resolve))
					.As<IMapper>()
					.InstancePerLifetimeScope();
		}
	}
}