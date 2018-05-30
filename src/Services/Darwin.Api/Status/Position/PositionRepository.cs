using System.Collections.Generic;

namespace Darwin.Api.Status.Position
{
	public interface IPositionRepository
	{
		Position GetById(int id);
		IDictionary<int, Position> GetAll();
	}

	public class PositionRepository : IPositionRepository
	{
		private readonly Dictionary<int, Position> _positionMap;

		public PositionRepository()
		{
			_positionMap = new Dictionary<int, Position>()
			{
				{1, new Position { X = 5, Y = 7 }},
				{2, new Position { X = 2, Y = 2 }},
			};
		}

		public Position GetById(int id)
		{
			return _positionMap[id];
		}

		public IDictionary<int, Position> GetAll()
		{
			return _positionMap;
		}
	}
}