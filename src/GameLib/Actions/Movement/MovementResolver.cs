using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Entities;
using GameLib.Properties;
using GameLib.Logging;

namespace GameLib.Actions.Movement
{
	public class MovementResolver : IResolver
	{
		private readonly ILogger _logger;
		private readonly IFeedbackWriter _feedbackWriter;
		private readonly IPlayerRepository _playerRepository;
		private readonly IPlayArea _playArea;
		public string ActionName => "action.movement";

		public MovementResolver(ILogger logger, IFeedbackWriter feedbackWriter, IPlayerRepository playerRepository, IPlayArea playArea)
		{
			_logger = logger;
			_feedbackWriter = feedbackWriter;
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
				default:
					_logger.Warn("Invalid movement direction: " + direction);
					return;
			}

			if (IsValidPosition(currentPosition, futureX, futureY))
			{
				player.Position.Move(futureX, futureY);
				await _playerRepository.AddorUpdateAsync(player).ConfigureAwait(false);
				_playArea.GameMap.Remove(currentPosition.X, currentPosition.Y, player);
				_playArea.GameMap.Add(player.Position.X, player.Position.Y, player);
				_feedbackWriter.WriteSuccess(playerId, nameof(Action), $"Walked {direction.ToString().ToLower()}");
			}
			else
			{
				_feedbackWriter.WriteFailure(playerId, nameof(Action), "Crashed into a wall!");
			}
		}

		private bool IsValidPosition(Position currentPosition, int futureX, int futureY)
		{
			return _playArea.GameMap.IsWalkable(currentPosition.X + futureX, currentPosition.Y + futureY);
		}
	}
}