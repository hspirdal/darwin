using System;
using GameLib.Actions;
using GameLib.Actions.Combat;
using GameLib.Entities;
using GameLib.Logging;
using GameLib.Messaging;
using Stateless;
using Stateless.Graph;

namespace GameLib.Properties.Autonomy
{
	public class SimpleDefender : IAutonomous
	{
		private enum State { Idle, Explore, Combat, Dead }
		private readonly StateMachine<State, MessageTopic> _stateMachine;
		private readonly ILogger _logger;
		private readonly IActionInserter _actionInserter;
		private readonly Creature _self;
		private readonly StateMachine<State, MessageTopic>.TriggerWithParameters<string> _attackedByTrigger;
		private string _targetId { get; set; }

		public SimpleDefender(ILogger logger, IActionInserter actionInserter, Creature self)
		{
			_logger = logger;
			_actionInserter = actionInserter;
			_self = self;

			_stateMachine = new StateMachine<State, MessageTopic>(State.Idle);
			_targetId = string.Empty;

			_stateMachine.Configure(State.Idle)
				.Permit(MessageTopic.AttackedBy, State.Combat);

			_attackedByTrigger = _stateMachine.SetTriggerParameters<string>(MessageTopic.AttackedBy);

			_stateMachine.Configure(State.Combat)
				.Permit(MessageTopic.KilledBy, State.Dead)
				.Permit(MessageTopic.CombatantDissapears, State.Idle)
				.Permit(MessageTopic.CombatantFlees, State.Idle)
				.Permit(MessageTopic.CombatantDies, State.Idle)
				.OnEntryFrom(_attackedByTrigger, attackerId => OnEnteringCombat(attackerId))
				.OnActivate(PerformAttack)
				.OnExit(OnExitingCombat);

			_stateMachine.Configure(State.Dead)
				.OnEntry(() => _logger.Debug("Entering death.."))
				.OnExit(() => _logger.Debug("Exiting death.."));
		}

		public string Id => _self.Id;

		public void Receive(GameMessage message)
		{
			if (message.Topic == MessageTopic.AttackedBy)
			{
				_stateMachine.Fire(_attackedByTrigger, message.FromEntityId);
			}
			else
			{
				_stateMachine.Fire(message.Topic);
			}
		}

		public void Act()
		{
			_stateMachine.Activate();
			_stateMachine.Deactivate();
		}

		private void PerformAttack()
		{
			var attack = new AttackAction(_self.Id, _targetId);
			_actionInserter.Insert(attack);
		}

		private void OnEnteringCombat(string attackerId)
		{
			_targetId = attackerId;
		}

		private void OnExitingCombat()
		{
			_targetId = string.Empty;
		}
	}
}