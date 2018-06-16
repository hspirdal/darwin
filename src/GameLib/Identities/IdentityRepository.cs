using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameLib.Identities
{
	public interface IIdentityRepository
	{
		Task AddAsync(Identity identity);
		List<Identity> GetAll();
	}

	public class IdentityRepository : IIdentityRepository
	{
		private readonly IDatabase _database;
		private readonly string _partionKey = "identity";

		public IdentityRepository(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
		}

		public Task AddAsync(Identity identity)
		{
			return _database.HashSetAsync(_partionKey, identity.Id.ToString(), JsonConvert.SerializeObject(identity));
		}

		public List<Identity> GetAll()
		{
			var results = _database.HashGetAll(_partionKey);
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