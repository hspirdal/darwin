using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameServer.identity
{
	public interface IPlayerRepository
	{
		Task<Player> GetByIdAsync(int id);
		Task AddPlayerAsync(Player player);
	}

	public class PlayerRepository : IPlayerRepository
	{
		private readonly IDatabase _database;
		private readonly string _partionKey = "identity";
		public readonly Dictionary<int, Player> _playerMap;

		public PlayerRepository(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
		}

		public async Task<Player> GetByIdAsync(int id)
		{
			var result = await _database.HashGetAsync(_partionKey, id.ToString());
			if (result.HasValue)
			{
				return JsonConvert.DeserializeObject<Player>(result);
			}

			throw new ArgumentException($"No player stored with id {id}");
		}

		public Task AddPlayerAsync(Player player)
		{
			return _database.HashSetAsync(_partionKey, player.Id.ToString(), JsonConvert.SerializeObject(player));
		}
	}
}