using System;
using System.Threading.Tasks;
using GameServer.Actions;
using GameServer.Actions.Movement;
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
			var redis = connectionMultiplexer.GetDatabase();


			var actionRepository = new ActionRepository(connectionMultiplexer);
			var positionRepository = new PositionRepository(connectionMultiplexer);
			var movementResolver = new MovementResolver(positionRepository);

			// who needs precision or quickness >_>
			var nextGameTick = DateTime.UtcNow;
			while (true)
			{
				var currentTime = DateTime.UtcNow;
				if (currentTime > nextGameTick)
				{
					var diff = currentTime - nextGameTick;
					nextGameTick = DateTime.UtcNow.AddSeconds(1) - diff;

					var actions = await actionRepository.GetQueuedActionsAsync();
					var movementActions = actions.ConvertAll(list => (MovementAction)list);

					await movementResolver.ResolveAsync(movementActions).ConfigureAwait(false);
					await actionRepository.ClearActionsAsync().ConfigureAwait(false);
				}
			}
		}
	}
}
