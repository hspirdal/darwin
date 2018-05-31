using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Darwin.Api.Actions
{
	public interface IActionRepository
	{
		Task AddAsync(IAction action);
		Task<bool> AbleToAddAsync(int ownerIId);
		DateTime NextActionAvailable { get; }
	}

	public class ActionRepository : IActionRepository
	{
		private readonly IDatabase _database;
		private DateTime _lastCleared;
		private readonly string _partitionKey = "action";

		public ActionRepository(ConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
			_lastCleared = DateTime.UtcNow;

			TempClearRedisPartition();
		}

		public DateTime NextActionAvailable => _lastCleared.AddSeconds(1);

		public async Task AddAsync(IAction action)
		{
			var actionAlreadyStored = await _database.HashExistsAsync(_partitionKey, action.OwnerId.ToString());
			if (!actionAlreadyStored)
			{

				await _database.HashSetAsync(_partitionKey, action.OwnerId.ToString(), JsonConvert.SerializeObject(action));

				//_database.StringSet(action.OwnerId.ToString(), );
				return;
			}

			throw new ArgumentException($"Action already queued for this timeslot. [ownerId: {action.OwnerId}, action: {action.Name}");
		}

		public async Task<bool> AbleToAddAsync(int ownerIId)
		{
			var actionExists = await _database.HashExistsAsync(_partitionKey, ownerIId.ToString());
			return actionExists == false;
		}

		// Docker typically caches redis image for each startup, so make sure state is clear for now.
		private void TempClearRedisPartition()
		{
			var keys = _database.HashKeys(_partitionKey);
			_database.HashDelete(_partitionKey, keys);
		}
	}
}