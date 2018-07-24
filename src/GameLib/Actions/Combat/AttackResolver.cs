using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Logging;
using GameLib.Entities;
using GameLib.Combat;

namespace GameLib.Actions.Combat
{
	public class AttackResolver : IResolver
	{
		private readonly ILogger _logger;
		private readonly IFeedbackWriter _feedbackWriter;
		private readonly ICreatureRegistry _creatureRegistry;
		private readonly IPlayArea _playArea;
		private readonly ICombatSimulator _combatSimulator;

		public string ActionName => "action.attack";

		public AttackResolver(ILogger logger, IFeedbackWriter feedbackWriter, ICreatureRegistry creatureRegistry, IPlayArea playArea,
		ICombatSimulator combatSimulator)
		{
			_logger = logger;
			_feedbackWriter = feedbackWriter;
			_creatureRegistry = creatureRegistry;
			_playArea = playArea;
			_combatSimulator = combatSimulator;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var attackAction = (AttackAction)action;
			Resolve(attackAction.OwnerId, attackAction.TargetId);
			return Task.CompletedTask;
		}

		private void Resolve(string attackerId, string targetId)
		{
			var attacker = _creatureRegistry.GetById(attackerId);
			var cell = _playArea.GameMap.GetCell(attacker.Position.X, attacker.Position.Y);
			var target = cell.Content.Entities.SingleOrDefault(i => (i.Type == "Creature" || i.Type == "Player") && i.Id == targetId) as Creature;
			if (target != null)
			{
				_combatSimulator.PerformAttack(attacker, target);
			}
			else
			{
				_logger.Warn($"Creature '{attacker.Name}' failed to attack target with id '{targetId}'. It was either not found, or was not cast correctly.");
			}
		}
	}
}