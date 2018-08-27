using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Darwin.Api.Identities
{
	public interface IIdentityRepository
	{
		Task AddOrUpdateAsync(Identity identity);
		Task<Identity> GetByIdAsync(string id);
		Task<List<Identity>> GetAllAsync();
	}

	public class IdentityRepository : IIdentityRepository
	{
		private readonly IDatabase _database;
		private readonly string _partionKey = "identity";

		public IdentityRepository(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
		}

		public Task AddOrUpdateAsync(Identity identity)
		{
			return _database.HashSetAsync(_partionKey, identity.Id.ToString(), JsonConvert.SerializeObject(identity));
		}

		public async Task<Identity> GetByIdAsync(string id)
		{
			var result = await _database.HashGetAsync(_partionKey, id).ConfigureAwait(false);
			if (result.HasValue)
			{
				return JsonConvert.DeserializeObject<Identity>(result);
			}

			throw new ArgumentException($"Could not find identity with id {id}.");
		}

		public async Task<List<Identity>> GetAllAsync()
		{
			var results = await _database.HashGetAllAsync(_partionKey).ConfigureAwait(false);
			var identities = new List<Identity>();
			foreach (var result in results)
			{
				var identity = JsonConvert.DeserializeObject<Identity>(result.Value);
				identities.Add(identity);
			}

			return identities;
		}
	}
}