using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameLib
{
	public abstract class AbstractRedisRepository<T> where T : IIdentifiable
	{
		private readonly string _partitionKey;
		private readonly IDatabase _database;

		public AbstractRedisRepository(IConnectionMultiplexer connectionMultiplexer, string partitionKey)
		{
			_partitionKey = partitionKey;
			_database = connectionMultiplexer.GetDatabase();
		}

		public async Task<T> GetByIdAsync(string id)
		{
			ValidateId(id);

			var result = await _database.HashGetAsync(_partitionKey, id).ConfigureAwait(false);
			if (result.HasValue)
			{
				return JsonConvert.DeserializeObject<T>(result);
			}

			throw new ArgumentException($"Could not find result with id {id}.");
		}

		public async Task<List<T>> GetAllAsync()
		{
			var results = await _database.HashGetAllAsync(_partitionKey).ConfigureAwait(false);
			var objects = new List<T>();
			foreach (var result in results)
			{
				var obj = JsonConvert.DeserializeObject<T>(result.Value);
				objects.Add(obj);
			}

			return objects;
		}

		public Task AddOrUpdateAsync(T obj)
		{
			ValidateId(obj.Id);

			return _database.HashSetAsync(_partitionKey, obj.Id.ToString(), JsonConvert.SerializeObject(obj));
		}

		public Task RemoveAsync(string id)
		{
			ValidateId(id);

			return _database.HashDeleteAsync(_partitionKey, id);
		}

		private static void ValidateId(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("Id was either null or empty.");
			}
		}
	}
}