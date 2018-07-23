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
		private readonly IPlayerRepository _playerRepository;
		private readonly IPlayArea _playArea;
		private readonly ICombatRegistry _combatRegistry;

		public string ActionName => "action.attack";

		public AttackResolver(ILogger logger, IFeedbackWriter feedbackWriter, IPlayerRepository playerRepository, IPlayArea playArea,
		ICombatRegistry combatRegistry)
		{
			_logger = logger;
			_feedbackWriter = feedbackWriter;
			_playerRepository = playerRepository;
			_playArea = playArea;
			_combatRegistry = combatRegistry;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var lootAction = (AttackAction)action;
			return ResolveAsync(lootAction.OwnerId, lootAction.TargetId);
		}

		private async Task ResolveAsync(string playerId, string targetId)
		{
			var player = await _playerRepository.GetByIdAsync(playerId).ConfigureAwait(false);
			var cell = _playArea.GameMap.GetCell(player.Position.X, player.Position.Y);
			var target = cell.Content.Entities.SingleOrDefault(i => (i.Type == "Creature" || i.Type == "Player") && i.Id == targetId) as Creature;
			if (target != null)
			{
				_combatRegistry.Register(playerId, targetId);
				_logger.Info($"Player '{player.Name}' attacks target '{target.Name}'");
				_feedbackWriter.WriteSuccess(playerId, nameof(Action), $"Attacking {target.Name}");
			}
			else
			{
				_logger.Warn($"Player '{player.Name}' failed to attack target with id '{targetId}'. It was either not found, or was not cast correctly.");
			}
		}
	}
}