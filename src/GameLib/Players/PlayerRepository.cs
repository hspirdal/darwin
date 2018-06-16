using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameLib.Players
{
    public interface IPlayerRepository
    {
        Task<Player> GetByIdAsync(string id);
        Player GetById(string id);
        Task AddPlayerAsync(Player player);
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

        public Task AddPlayerAsync(Player player)
        {
            return _database.HashSetAsync(_partionKey, player.Id, JsonConvert.SerializeObject(player));
        }
    }
}