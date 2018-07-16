using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using GameLib.Properties;

namespace GameLib.Entities
{
	public interface IPlayerRepository
	{
		Task<Player> GetByIdAsync(string id);
		Player GetById(string id);
		Task<List<Player>> GetAllPlayersAsync();
		Task<List<Player>> GetAllActivePlayersAsync();

		Task AddorUpdateAsync(Player player);
	}

	public class PlayerRepository : IPlayerRepository
	{
		private readonly IDatabase _database;
		private readonly string _partionKey = "player";

		public PlayerRepository(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
		}

		public async Task<Player> GetByIdAsync(string id)
		{
			var result = await _database.HashGetAsync(_partionKey, id);
			if (result.HasValue)
			{
				return JsonConvert.DeserializeObject<Player>(result);
			}

			throw new ArgumentException($"No player stored with id {id}");
		}

		public Player GetById(string id)
		{
			var result = _database.HashGet(_partionKey, id);
			if (result.HasValue)
			{
				return JsonConvert.DeserializeObject<Player>(result);
			}

			throw new ArgumentException($"No player stored with id {id}");
		}

		public async Task<List<Player>> GetAllPlayersAsync()
		{
			var entries = await _database.HashGetAllAsync(_partionKey).ConfigureAwait(false);
			var players = new List<Player>();
			foreach (var entry in entries)
			{
				var player = JsonConvert.DeserializeObject<Player>(entry.Value);
				players.Add(player);
			}
			return players;
		}

		public async Task<List<Player>> GetAllActivePlayersAsync()
		{
			var players = await GetAllPlayersAsync().ConfigureAwait(false);
			return players.Where(i => i.GameState == GameState.InGame).ToList();
		}


		public Task AddorUpdateAsync(Player player)
		{
			return _database.HashSetAsync(_partionKey, player.Id, JsonConvert.SerializeObject(player));
		}
	}
}