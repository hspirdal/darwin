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

namespace WebSocketServer
{
	public interface IStartupTaskRunner
	{
		Task ExecuteAsync();
	}

	public class StartupTaskRunner : IStartupTaskRunner
	{
		private readonly IPlayerRepository _playerRepository;
		private readonly IIdentityRepository _identityRepository;
		private readonly IWeaponFactory _weaponFactory;

		public StartupTaskRunner(IPlayerRepository playerRepository, IIdentityRepository identityRepository, IWeaponFactory weaponFactory)
		{
			_playerRepository = playerRepository;
			_identityRepository = identityRepository;
			_weaponFactory = weaponFactory;
		}

		public async Task ExecuteAsync()
		{
			await CreateInitialIndentities().ConfigureAwait(false);
			await CreateInitialPlayers().ConfigureAwait(false);
		}

		private async Task CreateInitialPlayers()
		{
			var shortSword = _weaponFactory.Create("Short Sword");
			var fighterStats = new Statistics()
			{
				AbilityScores = new AbilityScores(16, 14, 16, 9, 9, 8),
				AttackScores = new AttackScores(shortSword, 1),
				DefenseScores = new DefenseScores(8, 12)
			};

			await _playerRepository.AddorUpdateAsync(new Player(id: "1", "Jools", "Human", "Fighter", level: 1, fighterStats)
			{
				Inventory = new Inventory
				{
					Items = new List<Item>
					{
						shortSword,
						new Item { Name = "Studded Leather", Id = Guid.NewGuid().ToString() }
					}
				}
			}).ConfigureAwait(false);

			var quarterStaff = _weaponFactory.Create("Quarterstaff");
			var wizardStats = new Statistics()
			{
				AbilityScores = new AbilityScores(8, 16, 12, 18, 9, 9),
				AttackScores = new AttackScores(quarterStaff, 0),
				DefenseScores = new DefenseScores(10, 6)
			};

			await _playerRepository.AddorUpdateAsync(new Player(id: "2", "Jops", "Human", "Wizard", level: 1, wizardStats)
			{
				Inventory = new Inventory
				{
					Items = new List<Item>
					{
						quarterStaff,
						new Item { Name = "Wizard Robes", Id = Guid.NewGuid().ToString() }
					}
				}
			}).ConfigureAwait(false);
		}

		private async Task CreateInitialIndentities()
		{
			await _identityRepository.AddAsync(new Identity { Id = "1", UserName = "arch", Password = "1234" }).ConfigureAwait(false);
			await _identityRepository.AddAsync(new Identity { Id = "2", UserName = "clip", Password = "1234" }).ConfigureAwait(false);
		}
	}
}