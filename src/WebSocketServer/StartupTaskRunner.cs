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

		public StartupTaskRunner(IPlayerRepository playerRepository, IIdentityRepository identityRepository)
		{
			_playerRepository = playerRepository;
			_identityRepository = identityRepository;
		}

		public async Task ExecuteAsync()
		{
			await CreateInitialIndentities().ConfigureAwait(false);
			await CreateInitialPlayers().ConfigureAwait(false);
		}

		private async Task CreateInitialPlayers()
		{
			await _playerRepository.AddorUpdateAsync(new Player
			{
				Id = "1",
				Name = "Jools",
				GameState = GameState.lobby,
				Statistics = new CharacterStatistics("Human", "Fighter", 1)
				{
					AbilityScores = new AbilityScores(16, 14, 16, 9, 9, 8),
					AttackScores = new AttackScores(),
					DefenseScores = new DefenseScores(8, 12)
				},
				Inventory = new Inventory
				{
					Items = new List<Item>
					{
						new Item { Name = "Rusty Sword", Id = Guid.NewGuid().ToString() },
						new Item { Name = "Studded Leather", Id = Guid.NewGuid().ToString() }
					}
				}
			}).ConfigureAwait(false);
			await _playerRepository.AddorUpdateAsync(new Player
			{
				Id = "2",
				Name = "Jops",
				GameState = GameState.lobby,
				Statistics = new CharacterStatistics("Human", "Wizard", 1)
				{
					AbilityScores = new AbilityScores(8, 16, 12, 18, 9, 9),
					AttackScores = new AttackScores(),
					DefenseScores = new DefenseScores(10, 6)
				},
				Inventory = new Inventory
				{
					Items = new List<Item>
					{
						new Item { Name = "Quarterstaff", Id = Guid.NewGuid().ToString() },
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