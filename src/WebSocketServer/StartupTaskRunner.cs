using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Entities;
using GameLib.Properties;
using GameLib.Identities;
using GameLib.Properties.Stats;
using GameLib;
using GameLib.Messaging;

namespace WebSocketServer
{
	public interface IStartupTaskRunner
	{
		Task ExecuteAsync();
	}

	public class StartupTaskRunner : IStartupTaskRunner
	{
		private readonly IIdentityRepository _identityRepository;
		private readonly IWeaponFactory _weaponFactory;
		private readonly ICreatureRegistry _creatureRegistry;
		private readonly IPlayArea _playArea;
		private readonly IClientSender _clientSender;
		private readonly IRecipientRegistry _recipientRegistry;

		public StartupTaskRunner(IIdentityRepository identityRepository, IWeaponFactory weaponFactory,
		ICreatureRegistry creatureRegistry, IPlayArea playArea, IClientSender clientSender, IRecipientRegistry recipientRegistry)
		{
			_identityRepository = identityRepository;
			_weaponFactory = weaponFactory;
			_creatureRegistry = creatureRegistry;
			_playArea = playArea;
			_clientSender = clientSender;
			_recipientRegistry = recipientRegistry;
		}

		public async Task ExecuteAsync()
		{
			await CreateInitialIndentitiesAsync().ConfigureAwait(false);
			CreateInitialPlayers();
		}

		private void CreateInitialPlayers()
		{
			var shortSword = _weaponFactory.Create("Short Sword");
			var fighterStats = new Statistics()
			{
				AbilityScores = new AbilityScores(16, 14, 16, 9, 9, 8),
				AttackScores = new AttackScores(shortSword, 1),
				DefenseScores = new DefenseScores(14, 112)
			};

			var jools = new Player(id: "1", "Jools", "Human", "Fighter", level: 1, fighterStats)
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
			_recipientRegistry.Register(new ClientMessageProxy(_clientSender, jools.Id));

			var quarterStaff = _weaponFactory.Create("Quarterstaff");
			var wizardStats = new Statistics()
			{
				AbilityScores = new AbilityScores(8, 16, 12, 18, 9, 9),
				AttackScores = new AttackScores(quarterStaff, 0),
				DefenseScores = new DefenseScores(10, 6)
			};

			var jops = new Player(id: "2", "Jops", "Human", "Wizard", level: 1, wizardStats)
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
			_recipientRegistry.Register(new ClientMessageProxy(_clientSender, jops.Id));
		}

		private void SpawnInRandomOpenCell(Player player)
		{
			var cell = _playArea.GameMap.GetRandomOpenCell();
			player.Position.SetPosition(cell.X, cell.Y);
			_playArea.GameMap.Add(cell.X, cell.Y, player);
		}

		private async Task CreateInitialIndentitiesAsync()
		{
			await _identityRepository.AddOrUpdateAsync(new Identity { Id = "1", UserName = "arch", Password = "1234" }).ConfigureAwait(false);
			await _identityRepository.AddOrUpdateAsync(new Identity { Id = "2", UserName = "clip", Password = "1234" }).ConfigureAwait(false);
		}
	}
}