using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace TcpGameServer.Actions.Movement
{
	public class Position
	{
		public int X { get; set; }
		public int Y { get; set; }

		public override string ToString()
		{
			return $"[X: {X}, {Y}]";
		}
	}

	public interface IPositionRepository
	{
		Task<Position> GetByIdAsync(string id);
		Task SetPositionAsync(string playerId, int x, int y);
		Task StorePositionAsync(string playerId, int x, int y);
	}

	public class PositionRepository : IPositionRepository
	{
		private readonly IDatabase _database;
		private readonly string _partitionKey = "position";

		public PositionRepository(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
			TempClearRedisPartition();
		}

		public async Task<Position> GetByIdAsync(string id)
		{
			var result = await _database.HashGetAsync(_partitionKey, id.ToString());
			if (result.HasValue)
			{
				return JsonConvert.DeserializeObject<Position>(result);
			}

			throw new ArgumentException($"No position stored with id {id}");
		}

		public async Task SetPositionAsync(string playerId, int x, int y)
		{
			var position = await GetByIdAsync(playerId).ConfigureAwait(false);
			var logStatement = $"Old pos: {position}";
			position.X = x;
			position.Y = y;
			logStatement += $" - New pos: {position}";
			Console.WriteLine(logStatement);

			await AddAsync(playerId, position).ConfigureAwait(false);
		}

		public Task StorePositionAsync(string playerId, int x, int y)
		{
			var position = new Position { X = x, Y = y };
			return AddAsync(playerId, position);
		}

		private Task AddAsync(string playerId, Position position)
		{
			return _database.HashSetAsync(_partitionKey, playerId.ToString(), JsonConvert.SerializeObject(position));
		}

		// Docker typically caches redis image for each startup, so make sure state is clear for now.
		private void TempClearRedisPartition()
		{
			var keys = _database.HashKeys(_partitionKey);
			_database.HashDelete(_partitionKey, keys);
		}
	}
}