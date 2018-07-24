using System;
using GameLib.Area;
using GameLib.Entities;
using GameLib.Logging;
using GameLib.Utility;

namespace GameLib.Combat
{
	public interface ICombatSimulator
	{
		void PerformAttack(Creature attacker, Creature defender);
	}
	public class CombatSimulator : ICombatSimulator
	{
		private readonly ILogger _logger;
		private readonly IFeedbackWriter _feedbackWriter;
		private readonly ICombatRegistry _combatRegistry;
		private readonly ICreatureRegistry _creatureRegistry;
		private readonly IRandomGenerator _randomGenerator;
		private readonly IPlayArea _playArea;

		public CombatSimulator(ILogger logger, IFeedbackWriter feedbackWriter, ICombatRegistry combatRegistry,
		ICreatureRegistry creatureRegistry, IRandomGenerator randomGenerator, IPlayArea playArea)
		{
			_logger = logger;
			_feedbackWriter = feedbackWriter;
			_combatRegistry = combatRegistry;
			_creatureRegistry = creatureRegistry;
			_randomGenerator = randomGenerator;
			_playArea = playArea;
		}

		// public void CalculateRound()
		// {
		// 	var entries = _combatRegistry.GetAll();
		// 	foreach (var entry in entries)
		// 	{
		// 		var attacker = _creatureRegistry.GetById(entry.AttackerId);
		// 		var defender = _creatureRegistry.GetById(entry.TargetId);

		// 		if (attacker.Statistics.DefenseScores.HitPoints.Current <= 0)
		// 		{
		// 			_combatRegistry.Remove(entry.AttackerId);
		// 			continue;
		// 		}

		// 		ResolveAttack(attacker, defender);
		// 	}
		// }

		public void PerformAttack(Creature attacker, Creature defender)
		{
			if (attacker.IsAlive)
			{
				ResolveAttack(attacker, defender);
			}
		}

		private void ResolveAttack(Creature attacker, Creature defender)
		{
			var attackRoll = new AttackRoll(attacker.Statistics.AttackScores.Primary, 3, 3);
			var armorClass = defender.Statistics.DefenseScores.ArmorClass;
			var hitPoints = defender.Statistics.DefenseScores.HitPoints;

			var toHit = _randomGenerator.D20() + attackRoll.ToHitModifier;
			if (toHit >= armorClass.Total)
			{
				var totalDamage = _randomGenerator.Roll(attackRoll.DiceType, attackRoll.TimesApplied) + attackRoll.DamageModifier;
				hitPoints.Current -= totalDamage;
				_feedbackWriter.WriteSuccess(attacker.Id, "Combat", $"Successfull attack! ToHit {toHit} against AC {armorClass.Total}. Damage: {totalDamage}.");
				if (hitPoints.Current <= 0)
				{
					_playArea.GameMap.Remove(defender.Position.X, defender.Position.Y, defender);
					_feedbackWriter.WriteSuccess(attacker.Id, "Combat", $"{defender.Name} dies of combat damage!");
				}
			}
			else
			{
				_feedbackWriter.WriteFailure(attacker.Id, "Combat", $"Missed attack! ToHit {toHit} against AC {armorClass.Total}.");
			}
		}
	}
}