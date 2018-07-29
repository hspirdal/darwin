using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Entities;
using GameLib.Properties;
using GameLib.Logging;
using GameLib.Combat;
using GameLib.Messaging;

namespace GameLib.Actions.Movement
{
	public class MovementResolver : IResolver
	{
		private readonly ILogger _logger;
		private readonly ICreatureRegistry _creatureRegistry;
		private readonly IPlayArea _playArea;
		private readonly ICombatRegistry _combatRegistry;
		private readonly IMessageDispatcher _messageDispatcher;

		public string ActionName => MovementAction.CanonicalName;

		public MovementResolver(ILogger logger, ICreatureRegistry creatureRegistry, IPlayArea playArea,
			ICombatRegistry combatRegistry, IMessageDispatcher messageDispatcher)
		{
			_logger = logger;
			_creatureRegistry = creatureRegistry;
			_playArea = playArea;
			_combatRegistry = combatRegistry;
			_messageDispatcher = messageDispatcher;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var movementAction = (MovementAction)action;
			Resolve(movementAction.OwnerId, movementAction.MovementDirection);
			return Task.CompletedTask;
		}

		private void Resolve(string creatureId, MovementDirection direction)
		{
			var creature = _creatureRegistry.GetById(creatureId);
			var currentPosition = creature.Position.DeepCopy();
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

			if (_combatRegistry.IsInCombat(creatureId))
			{
				_messageDispatcher.Dispatch(new GameMessage(string.Empty, creature.Id, MessageTopic.MovementFailed,
					"You are locked in combat and cannot walk away."));
				return;
			}

			if (IsValidPosition(currentPosition, futureX, futureY))
			{
				creature.Position.Move(futureX, futureY);
				_playArea.GameMap.Remove(currentPosition.X, currentPosition.Y, creature);
				_playArea.GameMap.Add(creature.Position.X, creature.Position.Y, creature);
			}
			else
			{
				_messageDispatcher.Dispatch(new GameMessage(string.Empty, creature.Id, MessageTopic.MovementFailed, "You crashed into a wall!"));
			}
		}

		private bool IsValidPosition(Position currentPosition, int futureX, int futureY)
		{
			return _playArea.GameMap.IsWalkable(currentPosition.X + futureX, currentPosition.Y + futureY);
		}
	}
}