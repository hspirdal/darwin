using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Players;
using System.Linq;
using GameLib.Logging;

namespace GameLib.Actions.Loot
{
	public class LootAllResolver : IResolver
	{
		private readonly ILogger _logger;
		private readonly IPlayerRepository _playerRepository;
		private readonly IPlayArea _playArea;
		public string ActionName => "action.lootall";

		public LootAllResolver(ILogger logger, IPlayerRepository playerRepository, IPlayArea playArea)
		{
			_logger = logger;
			_playerRepository = playerRepository;
			_playArea = playArea;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var lootAction = (LootAllAction)action;
			return ResolveAsync(lootAction.OwnerId);
		}

		private async Task ResolveAsync(string playerId)
		{
			var player = await _playerRepository.GetByIdAsync(playerId).ConfigureAwait(false);
			var cell = _playArea.GameMap.GetCell(player.Position.X, player.Position.Y);
			foreach (var item in cell.Content.Entities.Where(i => i.Type == "Item").Cast<Item>().ToList())
			{
				if (item != null)
				{
					cell.Content.Entities.Remove(item);
					player.Inventory.Items.Add(item);
					_logger.Info($"Player '{player.Name}' looted item '{item.Name}'");
				}
			}
			await _playerRepository.AddorUpdateAsync(player).ConfigureAwait(false);
		}
	}
}