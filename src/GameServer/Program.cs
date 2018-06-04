using System;
using System.Threading.Tasks;
using GameServer.Actions;
using GameServer.Actions.Movement;
using GameServer.identity;
using StackExchange.Redis;

namespace GameServer
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var configuration = new ConfigurationOptions { ResolveDns = true };
			configuration.EndPoints.Add(Environment.GetEnvironmentVariable("RedisHost"));
			var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);

			var actionRepository = new ActionRepository(connectionMultiplexer);
			var playerRepository = new PlayerRepository(connectionMultiplexer);
			var positionRepository = new PositionRepository(connectionMultiplexer);
			var movementResolver = new MovementResolver(positionRepository);

			await CreateInitialPlayers(playerRepository).ConfigureAwait(false);
			await CreateInitialPositions(positionRepository).ConfigureAwait(false);
			await actionRepository.ClearActionsAsync().ConfigureAwait(false);

			// who needs precision or quickness >_>
			var nextGameTick = DateTime.UtcNow;
			while (true)
			{
				var currentTime = DateTime.UtcNow;
				if (currentTime > nextGameTick)
				{
					var diff = currentTime - nextGameTick;
					nextGameTick = DateTime.UtcNow.AddSeconds(1) - diff;

					await actionRepository.SetNextResolveTimeAsync(nextGameTick).ConfigureAwait(false);

					var actions = await actionRepository.GetQueuedActionsAsync();
					var movementActions = actions.ConvertAll(list => (MovementAction)list);

					await movementResolver.ResolveAsync(movementActions).ConfigureAwait(false);
					await actionRepository.ClearActionsAsync().ConfigureAwait(false);
				}
			}
		}

		private static async Task CreateInitialPlayers(IPlayerRepository playerRepository)
		{
			await playerRepository.AddPlayerAsync(new Player { Id = 1, Name = "Jools" }).ConfigureAwait(false);
			await playerRepository.AddPlayerAsync(new Player { Id = 2, Name = "Jops" }).ConfigureAwait(false);
		}

		private static async Task CreateInitialPositions(PositionRepository positionRepository)
		{
			await positionRepository.StorePositionAsync(1, 3, 7).ConfigureAwait(false);
			await positionRepository.StorePositionAsync(2, 8, 4).ConfigureAwait(false);
		}


	}
}
