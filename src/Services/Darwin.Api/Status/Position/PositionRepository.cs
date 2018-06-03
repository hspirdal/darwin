using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Darwin.Api.Status.Position
{
	public interface IPositionRepository
	{
		Task<Position> GetByIdAsync(int id);
		Task<List<PositionTuple>> GetAllAsync();
	}

	public class PositionRepository : IPositionRepository
	{
		private readonly IDatabase _database;
		private readonly string _partitionKey = "position";

		public PositionRepository(ConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
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

		public async Task<List<PositionTuple>> GetAllAsync()
		{
			var allResults = await _database.HashGetAllAsync(_partitionKey);

			var positions = new List<PositionTuple>();
			foreach (var result in allResults)
			{
				Console.WriteLine(result);
				var position = JsonConvert.DeserializeObject<Position>(result.Value);
				var ownerId = int.Parse(result.Name);
				positions.Add(new PositionTuple { OwnerId = ownerId, Position = position });
			}

			return positions;
		}
	}
}