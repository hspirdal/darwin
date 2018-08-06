using System;
using System.Collections.Generic;
using GameLib.Area;
using GameLib.Messaging;
using GameLib.Properties;
using GameLib.Properties.Stats;

namespace GameLib.Entities
{
	public interface IPremadeCharacterSpawner
	{
		void Spawn(string userId, string templateName);
	}

	public class PremadeCharacterSpawner : IPremadeCharacterSpawner
	{
		private readonly IWeaponFactory _weaponFactory;
		private readonly IPlayArea _playArea;
		private readonly ICreatureRegistry _creatureRegistry;

		public PremadeCharacterSpawner(IWeaponFactory weaponFactory, IPlayArea playArea, ICreatureRegistry creatureRegistry)
		{
			_weaponFactory = weaponFactory;
			_playArea = playArea;
			_creatureRegistry = creatureRegistry;
		}
		public void Spawn(string userId, string templateName)
		{
			if (templateName == "Jools")
			{
				SpawnFirstLevelFighter(userId);
			}
			else if (templateName == "Jops")
			{
				SpawnFirstLevelWizard(userId);
			}
			else
			{
				throw new ArgumentException($"Template name '{templateName}'was not found.");
			}
		}

		private void SpawnFirstLevelFighter(string userId)
		{
			var shortSword = _weaponFactory.Create("Short Sword");
			var fighterStats = new Statistics()
			{
				AbilityScores = new AbilityScores(16, 14, 16, 9, 9, 8),
				AttackScores = new AttackScores(shortSword, 1),
				DefenseScores = new DefenseScores(14, 11002)
			};

			var jools = new Player(id: userId, "Jools", "Human", "Fighter", level: 1, fighterStats)
			{
				Inventory = new Inventory
				{
					Items = new List<Item>
					{
						shortSword,
						new Item { Name = "Studded Leather", Id = Guid.NewGuid().ToString() }
					}
				}
			};
			SpawnInRandomOpenCell(jools);
			_creatureRegistry.Register(jools);
		}

		private void SpawnFirstLevelWizard(string userId)
		{
			var quarterStaff = _weaponFactory.Create("Quarterstaff");
			var wizardStats = new Statistics()
			{
				AbilityScores = new AbilityScores(8, 16, 12, 18, 9, 9),
				AttackScores = new AttackScores(quarterStaff, 0),
				DefenseScores = new DefenseScores(10, 6)
			};

			var jops = new Player(id: userId, "Jops", "Human", "Wizard", level: 1, wizardStats)
			{
				Inventory = new Inventory
				{
					Items = new List<Item>
					{
						quarterStaff,
						new Item { Name = "Wizard Robes", Id = Guid.NewGuid().ToString() }
					}
				}
			};
			SpawnInRandomOpenCell(jops);
			_creatureRegistry.Register(jops);
		}

		private void SpawnInRandomOpenCell(Player player)
		{
			var cell = _playArea.GameMap.GetRandomOpenCell();
			player.Position.SetPosition(cell.X, cell.Y);
			_playArea.GameMap.Add(cell.X, cell.Y, player);
		}
	}
}