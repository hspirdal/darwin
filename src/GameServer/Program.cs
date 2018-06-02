using System;
using System.Threading.Tasks;
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
					foreach (var action in actions)
					{
						action.Resolve();
					}

					await actionRepository.ClearActionsAsync();
				}
			}
		}
	}
}
