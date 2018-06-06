using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using StackExchange.Redis;
using TcpGameServer.Actions.Movement;

namespace TcpGameServer.Actions
{
	public interface IActionRepository
	{
		Task<List<IAction>> GetQueuedActionsAsync();
		Task SetNextResolveTimeAsync(DateTime dateTime);
		Task ClearActionsAsync();
	}

	public class ActionRepository : IActionRepository
	{
		private readonly IDatabase _database;
		private readonly string _partitionKey = "action";

		public ActionRepository(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}

		public async Task<List<IAction>> GetQueuedActionsAsync()
		{
			var allValues = await _database.HashGetAllAsync(_partitionKey);

			var actions = new List<IAction>();
			foreach (var item in allValues)
			{
				Console.WriteLine(item);
				var action = JsonConvert.DeserializeObject<MovementAction>(item.Value);
				actions.Add(action);
			}

			return actions;
		}

		public async Task ClearActionsAsync()
		{
			var keys = await _database.HashKeysAsync(_partitionKey);
			await _database.HashDeleteAsync(_partitionKey, keys);
		}

		public Task SetNextResolveTimeAsync(DateTime dateTime)
		{
			return _database.HashSetAsync($"{_partitionKey}.meta", "nextResolveTime", JsonConvert.SerializeObject(dateTime));
		}
	}
}