using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLib.Actions;
using GameLib.Area;

namespace GameLib.Actions.Movement
{
	public class MovementResolver : IResolver
	{
		private readonly IPositionRepository _positionRepository;
		private readonly PlayArea _playArea;
		public string ActionName => "action.movement";

		public MovementResolver(IPositionRepository positionRepository, PlayArea playArea)
		{
			_positionRepository = positionRepository;
			_playArea = playArea;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var movementAction = (MovementAction)action;
			return ResolveAsync(movementAction.OwnerId, movementAction.MovementDirection);
		}

		private async Task ResolveAsync(string playerId, MovementDirection direction)
		{
			var currentPosition = await _positionRepository.GetByIdAsync(playerId).ConfigureAwait(false);
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
				await _positionRepository.SetPositionAsync(playerId, currentPosition.X + futureX, currentPosition.Y + futureY).ConfigureAwait(false);
			}
		}

		private bool IsValidPosition(Position currentPosition, int futureX, int futureY)
		{
			var cell = _playArea.Map.GetCell(currentPosition.X + futureX, currentPosition.Y + futureY);
			return cell != null && cell.IsWalkable;
		}
	}
}