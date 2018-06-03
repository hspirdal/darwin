using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameServer.Actions.Movement
{
	public class Position
	{
		public int X { get; set; }
		public int Y { get; set; }
	}

	public interface IPositionRepository
	{
		Task<Position> GetByIdAsync(int id);
		Task SetPositionAsync(int playerId, int x, int y);
	}

	public class PositionRepository : IPositionRepository
	{
		private readonly IDatabase _database;
		private readonly string _partitionKey = "position";
		private readonly Dictionary<int, Position> _positionMap;

		public PositionRepository(ConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();

			_positionMap = new Dictionary<int, Position>()
			{
				{1, new Position { X = 5, Y = 7 }},
				{2, new Position { X = 2, Y = 2 }},
			};
		}

		public async Task<Position> GetByIdAsync(int id)
		{
			var result = await _database.HashGetAsync(_partitionKey, id.ToString());
			if (result.HasValue)
			{
				return JsonConvert.DeserializeObject<Position>(result);
			}

			throw new ArgumentException($"No position stored with id {id}");
		}

		public async Task SetPositionAsync(int playerId, int x, int y)
		{
			var position = await GetByIdAsync(playerId).ConfigureAwait(false);
			position.X = x;
			position.Y = y;

			var success = await _database.HashSetAsync(_partitionKey, playerId.ToString(), JsonConvert.SerializeObject(position)).ConfigureAwait(false);
			if (!success)
			{
				throw new InvalidOperationException($"Failed to store position in redis for id {playerId}");
			}
		}
	}
}