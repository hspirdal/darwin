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
using GameLib.Area;
using GameLib.Actions.Loot;
using GameLib.Properties.Stats;
using GameLib.Utility;
using GameLib.Actions.Combat;
using GameLib.Combat;
using GameLib.Properties.Autonomy;
using GameLib.Messaging;
using GameLib;
using GameLib.Dice;
using GameLib.Users;

namespace WebSocketServer
{
	public class Builder
	{
		public static IContainer CreateContainer()
		{
			var builder = new ContainerBuilder();

			RegisterAutoMapper(builder);
			RegisterRedis(builder);
			RegisterActionFactoryRegistry(builder);
			RegisterActionResolver(builder);
			RegisterCreatureFactory(builder);
			RegisterWeaponFactory(builder);
			RegisterPlayArea(builder);
			RegisterGameConfigurations(builder);

			builder.RegisterType<ConnectionRegistry>().As<IConnectionRegistry>().SingleInstance();
			builder.RegisterType<SocketServer>().As<ISocketServer>().As<IClientRegistry>().As<IClientSender>().SingleInstance();
			builder.RegisterType<Logger>().As<ILogger>();
			builder.RegisterType<Clock>().As<IClock>();
			builder.RegisterType<ActionRepository>().As<IActionRepository>().SingleInstance();
			builder.RegisterType<Authenticator>().As<IAuthenticator>();
			builder.RegisterType<MapGenerator>().As<IMapGenerator>();
			builder.RegisterType<StartupTaskRunner>().As<IStartupTaskRunner>();
			builder.RegisterType<GameServer>().As<IGameServer>();
			builder.RegisterType<LobbyRouter>().As<ILobbyRouter>();
			builder.RegisterType<GameRouter>().As<IGameRouter>();
			builder.RegisterType<StateRequestRouter>().As<IStateRequestRouter>();
			builder.RegisterType<ActionInserter>().As<IActionInserter>();
			builder.RegisterType<RandomNumberGenerator>().As<IRandomNumberGenerator>();
			builder.RegisterType<DiceRoller>().As<IDiceRoller>();
			builder.RegisterType<CreatureRegistry>().As<ICreatureRegistry>().SingleInstance();
			builder.RegisterType<CombatRegistry>().As<ICombatRegistry>().SingleInstance();
			builder.RegisterType<CooldownRegistry>().As<ICooldownRegistry>().SingleInstance();
			builder.RegisterType<AutonomousRegistry>().As<IAutonomousRegistry>().SingleInstance();
			builder.RegisterType<RecipientRegistry>().As<IRecipientRegistry>().SingleInstance();
			builder.RegisterType<CombatSimulator>().As<ICombatSimulator>();
			builder.RegisterType<MessageDispatcher>().As<IMessageDispatcher>();
			builder.RegisterType<AutonomousFactory>().As<IAutonomousFactory>();
			builder.RegisterType<PremadeCharacterSpawner>().As<IPremadeCharacterSpawner>();
			builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();

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

		private static void RegisterActionFactoryRegistry(ContainerBuilder builder)
		{
			builder.Register<ActionFactoryRegistry>(c =>
			{
				var movementactionFactory = new MovementActionFactory();
				var lootActionFactory = new LootActionFactory();
				var lootAllActionFactory = new LootAllActionFactory();
				var attackActionFactory = new AttackActionFactory();

				var actionFactoryMap = new Dictionary<string, ActionFactory>
				{
					{movementactionFactory.ActionName, movementactionFactory},
					{lootActionFactory.ActionName, lootActionFactory},
					{lootAllActionFactory.ActionName, lootAllActionFactory},
					{attackActionFactory.ActionName, attackActionFactory},
				};
				return new ActionFactoryRegistry(actionFactoryMap);
			}).As<IActionFactoryRegistry>().SingleInstance();
		}

		private static void RegisterActionResolver(ContainerBuilder builder)
		{
			builder.Register<ActionResolver>(c =>
			{
				var movementResolver = new MovementResolver(c.Resolve<ILogger>(), c.Resolve<ICreatureRegistry>(), c.Resolve<IPlayArea>(), c.Resolve<ICombatRegistry>(), c.Resolve<IMessageDispatcher>());
				var lootAllResolver = new LootAllResolver(c.Resolve<ILogger>(), c.Resolve<ICreatureRegistry>(), c.Resolve<IPlayArea>());
				var lootResolver = new LootResolver(c.Resolve<ILogger>(), c.Resolve<ICreatureRegistry>(), c.Resolve<IPlayArea>());
				var attackResolver = new AttackResolver(c.Resolve<ILogger>(), c.Resolve<ICreatureRegistry>(), c.Resolve<IPlayArea>(), c.Resolve<ICombatSimulator>());
				var resolverMap = new Dictionary<string, IResolver>
					{
						{ movementResolver.ActionName, movementResolver },
						{ lootAllResolver.ActionName, lootAllResolver },
						{ lootResolver.ActionName, lootResolver },
						{ attackResolver.ActionName, attackResolver }
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

				var creatureSpawner = new CreatureSpawner(playArea, c.Resolve<ICreatureFactory>(), c.Resolve<ICreatureRegistry>(),
					c.Resolve<IAutonomousFactory>(), c.Resolve<IAutonomousRegistry>(), c.Resolve<IRecipientRegistry>());
				var totalCreaturesToAdd = (int)((playArea.GameMap.Width * playArea.GameMap.Height) * 0.01);
				creatureSpawner.SpawnRandomly(totalCreaturesToAdd);
				return playArea;
			}).As<IPlayArea>().SingleInstance();
		}

		private static void RegisterCreatureFactory(ContainerBuilder builder)
		{
			builder.Register((Func<IComponentContext, CreatureFactory>)(c =>
			{
				var weaponFactory = c.Resolve<IWeaponFactory>();

				var templates = new List<Creature>
				{
					new Creature(Guid.NewGuid().ToString(), "Orc Warrior", "Orc", "Fighter", new Statistics{
						AbilityScores = new AbilityScores(17, 11, 12, 7, 8, 6),
						AttackScores = new AttackScores(weaponFactory.Create("Falchion"), 1),
						DefenseScores = new DefenseScores(13, 6)
					}),
					new Creature(Guid.NewGuid().ToString(), "Dire Bat", "Animal", string.Empty, new Statistics{
						AbilityScores = new AbilityScores(17, 15, 13, 2, 14, 6),
						AttackScores = new AttackScores(weaponFactory.Create("Bat Bite"), 3),
						DefenseScores = new DefenseScores(14, 22)
					}),
					new Creature(Guid.NewGuid().ToString(), "Goblin", "Goblinoid", "Fighter", new Statistics{
						AbilityScores = new AbilityScores(11, 15, 12, 10, 9, 6),
						AttackScores = new AttackScores(weaponFactory.Create("Short Sword"), 1),
						DefenseScores = new DefenseScores(16, 6)
					}),
					new Creature(Guid.NewGuid().ToString(), "Grizzly Bear", "Animal", string.Empty, new Statistics{
						AbilityScores = new AbilityScores(21, 13, 19, 2, 12, 6),
						AttackScores = new AttackScores(weaponFactory.Create("Bear Bite"), 3),
						DefenseScores = new DefenseScores(16, 42)
					}),
				};

				return new CreatureFactory((List<Creature>)templates, (IRandomNumberGenerator)c.Resolve<GameLib.Utility.IRandomNumberGenerator>());
			})).As<ICreatureFactory>().SingleInstance();
		}

		private static void RegisterWeaponFactory(ContainerBuilder builder)
		{
			builder.Register<WeaponFactory>(c =>
			{
				var templates = new List<Weapon>
				{
					new Weapon(Guid.NewGuid().ToString(), "Short Sword", DamageType.Piercing, DiceType.d6, 1, 2, 19 ),
					new Weapon(Guid.NewGuid().ToString(), "Quarterstaff", DamageType.Bludgeoning, DiceType.d6, 1, 2, 20 ),
					new Weapon(Guid.NewGuid().ToString(), "Bear Bite", DamageType.Slashing, DiceType.d6, 1, 2, 20 ),
					new Weapon(Guid.NewGuid().ToString(), "Bat Bite", DamageType.Slashing, DiceType.d8, 1, 2, 20 ),
					new Weapon(Guid.NewGuid().ToString(), "Falchion", DamageType.Slashing, DiceType.d4, 2, 2, 18 ),
				};
				return new WeaponFactory(templates);
			}).As<IWeaponFactory>().SingleInstance();
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