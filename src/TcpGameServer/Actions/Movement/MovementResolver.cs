using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TcpGameServer.Actions.Movement
{
	public interface IMovementResolver
	{
		Task ResolveAsync(List<MovementAction> actions);
	}

	public class MovementResolver : IMovementResolver
	{
		private readonly IPositionRepository _positionRepository;

		public MovementResolver(IPositionRepository positionRepository)
		{
			_positionRepository = positionRepository;
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

		private async Task ResolveAsync(int playerId, MovementDirection direction)
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
			return true;
		}
	}
}