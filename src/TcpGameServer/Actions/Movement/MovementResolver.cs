using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TcpGameServer.Area;

namespace TcpGameServer.Actions.Movement
{
	public interface IMovementResolver
	{
		Task ResolveAsync(List<MovementAction> actions);
	}

	public class MovementResolver : IMovementResolver
	{
		private readonly IPositionRepository _positionRepository;
		private readonly PlayArea _playArea;

		public MovementResolver(IPositionRepository positionRepository, PlayArea playArea)
		{
			_positionRepository = positionRepository;
			_playArea = playArea;
		}

		public async Task ResolveAsync(List<MovementAction> actions)
		{
			if (actions.Count > 0)
			{
				Console.WriteLine($"Resolving {actions.Count} items");
			}

			foreach (var action in actions)
			{
				// TODO: Task.WhenAll
				await ResolveAsync(action.OwnerId, action.MovementDirection).ConfigureAwait(false);
			}
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

			Console.WriteLine("Validationg..");
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