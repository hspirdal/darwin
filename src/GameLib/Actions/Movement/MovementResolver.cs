using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Players;

namespace GameLib.Actions.Movement
{
	public class MovementResolver : IResolver
	{
		private readonly IPlayerRepository _playerRepository;
		private readonly IPlayArea _playArea;
		public string ActionName => "action.movement";

		public MovementResolver(IPlayerRepository playerRepository, IPlayArea playArea)
		{
			_playerRepository = playerRepository;
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
			var player = await _playerRepository.GetByIdAsync(playerId).ConfigureAwait(false);
			var currentPosition = player.Position.Clone();
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
				player.Position.Move(futureX, futureY);
				await _playerRepository.AddorUpdateAsync(player).ConfigureAwait(false);
				_playArea.GameMap.Remove(currentPosition.X, currentPosition.Y, player);
				_playArea.GameMap.Add(player.Position.X, player.Position.Y, player);
			}
		}

		private bool IsValidPosition(Position currentPosition, int futureX, int futureY)
		{
			return _playArea.GameMap.IsWalkable(currentPosition.X + futureX, currentPosition.Y + futureY);
		}
	}
}