using System;
using GameLib.Area;
using GameLib.Dice;
using GameLib.Entities;
using GameLib.Logging;
using GameLib.Messaging;
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
		private readonly IDiceRoller _diceRoller;
		private readonly IPlayArea _playArea;
		private readonly IMessageDispatcher _messageDispatcher;

		public CombatSimulator(ILogger logger, IFeedbackWriter feedbackWriter, ICombatRegistry combatRegistry,
		ICreatureRegistry creatureRegistry, IDiceRoller diceRoller, IPlayArea playArea, IMessageDispatcher messageDispatcher)
		{
			_logger = logger;
			_feedbackWriter = feedbackWriter;
			_combatRegistry = combatRegistry;
			_creatureRegistry = creatureRegistry;
			_diceRoller = diceRoller;
			_playArea = playArea;
			_messageDispatcher = messageDispatcher;
		}
		public void PerformAttack(Creature attacker, Creature defender)
		{
			if (attacker.IsAlive)
			{
				if (!_combatRegistry.IsInCombat(attacker.Id))
				{
					_combatRegistry.Register(attacker.Id, defender.Id);
					_feedbackWriter.Write(attacker.Id, "Combat", $"Attacking {defender.Name}");
				}

				ResolveAttack(attacker, defender);
			}
		}

		private void ResolveAttack(Creature attacker, Creature defender)
		{
			var attackRoll = new AttackRoll(attacker.Statistics.AttackScores.Primary, 3, 3);
			var armorClass = defender.Statistics.DefenseScores.ArmorClass;
			var hitPoints = defender.Statistics.DefenseScores.HitPoints;

			var toHit = _diceRoller.D20().Result + attackRoll.ToHitModifier;
			if (toHit >= armorClass.Total)
			{
				var totalDamage = _diceRoller.Roll(attackRoll.DiceType, attackRoll.TimesApplied).Total + attackRoll.DamageModifier;
				hitPoints.Current -= totalDamage;
				_feedbackWriter.WriteSuccess(attacker.Id, "Combat", $"Successfull attack! ToHit {toHit} against AC {armorClass.Total}. Damage: {totalDamage}.");
				if (hitPoints.Current <= 0)
				{
					_combatRegistry.Remove(attacker.Id);
					_combatRegistry.Remove(defender.Id);
					_playArea.GameMap.Remove(defender.Position.X, defender.Position.Y, defender);
					_messageDispatcher.Dispatch(new GameMessage(attacker.Id, defender.Id, MessageTopic.KilledBy));
					_messageDispatcher.Dispatch(new GameMessage(defender.Id, attacker.Id, MessageTopic.CombatantDies));

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