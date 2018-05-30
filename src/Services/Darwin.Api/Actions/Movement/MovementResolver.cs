using System;
using Darwin.Api.Status.Position;

namespace Darwin.Api.Actions.Movement
{
	public interface IMovementResolver
	{
		void Resolve(int playerId, MovementDirection direction);
	}

	public class MovementResolver : IMovementResolver
	{
		private readonly IPositionRepository _positionRepository;

		public MovementResolver(IPositionRepository positionRepository)
		{
			_positionRepository = positionRepository;
		}

		public void Resolve(int playerId, MovementDirection direction)
		{
			var currentPosition = _positionRepository.GetById(playerId);
			var futureX = 0;
			var futureY = 0;

			switch (direction)
			{
				case MovementDirection.West:
					futureX = -1;
					break;
				case MovementDirection.East:
					futureX = 1;
					break;
				case MovementDirection.North:
					futureY = -1;
					break;
				case MovementDirection.South:
					futureY = 1;
					break;
			}

			if (IsValidPosition(currentPosition, futureX, futureY))
			{
				_positionRepository.SetPosition(playerId, currentPosition.X + futureX, currentPosition.Y + futureY);
			}
		}

		private bool IsValidPosition(Position currentPosition, int futureX, int futureY)
		{
			return true;
		}
	}
}