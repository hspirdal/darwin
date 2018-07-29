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
			if (attacker.IsAlive && defender.IsAlive)
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

			var attackResult = new AttackResult(attacker.Id, defender.Id, attackRoll.ToHitModifier, attackRoll.DamageModifier);
			attackResult.AttackerName = attacker.Name;
			attackResult.DefenderName = defender.Name;
			attackResult.ToHitRoll = _diceRoller.D20().Result;

			var hitResult = MessageTopic.FailedHitBy;
			if (attackResult.ToHitTotal >= armorClass.Total)
			{
				attackResult.SuccessfulHit = true;
				attackResult.DamageRoll = _diceRoller.Roll(attackRoll.DiceType, attackRoll.TimesApplied).Total;

				hitPoints.Current -= attackResult.DamageTotal;
				hitResult = MessageTopic.SuccessfulHitBy;
				_feedbackWriter.WriteSuccess(attacker.Id, "Combat", $"Successfull attack! ToHit {attackResult.ToHitTotal} against AC {armorClass.Total}. Damage: {attackResult.DamageTotal}.");
			}
			else
			{
				_feedbackWriter.WriteFailure(attacker.Id, "Combat", $"Missed attack! ToHit {attackResult.ToHitTotal} against AC {armorClass.Total}.");
			}

			var messageToDefender = new GameMessage(attacker.Id, defender.Id, hitResult, attackResult);
			var messageToAttacker = new GameMessage(defender.Id, attacker.Id, hitResult, attackResult);
			_messageDispatcher.Dispatch(messageToDefender);
			_messageDispatcher.Dispatch(messageToAttacker);

			if (hitPoints.Current <= 0)
			{
				_combatRegistry.Remove(attacker.Id);
				_combatRegistry.Remove(defender.Id);
				_playArea.GameMap.Remove(defender.Position.X, defender.Position.Y, defender);
				_messageDispatcher.Dispatch(new GameMessage(attacker.Id, defender.Id, MessageTopic.KilledBy, attackResult));
				_messageDispatcher.Dispatch(new GameMessage(defender.Id, attacker.Id, MessageTopic.CombatantDies, attackResult));

				// Temp for mockup reasons..
				if (attacker.Type == "Player")
				{
					_messageDispatcher.Dispatch(new GameMessage(string.Empty, attacker.Id, MessageTopic.ExperienceGain));
				}

				_feedbackWriter.WriteSuccess(attacker.Id, "Combat", $"{defender.Name} dies of combat damage!");
			}
		}
	}
}