using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Logging;
using GameLib.Entities;

namespace GameLib.Actions.Loot
{
	public class LootAllResolver : IResolver
	{
		private readonly ILogger _logger;
		private readonly ICreatureRegistry _creatureRegistry;
		private readonly IPlayArea _playArea;
		public string ActionName => LootAllAction.CanonicalName;

		public LootAllResolver(ILogger logger, ICreatureRegistry creatureRegistry, IPlayArea playArea)
		{
			_logger = logger;
			_creatureRegistry = creatureRegistry;
			_playArea = playArea;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var lootAction = (LootAllAction)action;
			Resolve(lootAction.OwnerId);
			return Task.CompletedTask;
		}

		private void Resolve(string creatureId)
		{
			var creature = _creatureRegistry.GetById(creatureId) as Player; // tmp cast;
			var cell = _playArea.GameMap.GetCell(creature.Position.X, creature.Position.Y);
			foreach (var item in cell.Content.Entities.Where(i => i.Type == "Item").Cast<Item>().ToList())
			{
				if (item != null)
				{
					cell.Content.Entities.Remove(item);
					creature.Inventory.Items.Add(item);
					_logger.Info($"Player '{creature.Name}' looted item '{item.Name}'");
				}
			}
		}
	}
}