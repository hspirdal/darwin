using System.Threading.Tasks;
using TcpGameServer.Actions;
using TcpGameServer.Actions.Movement;
using TcpGameServer.Players;

namespace TcpGameServer
{
	public interface IStartupTaskRunner
	{
		Task ExecuteAsync();
	}

	public class StartupTaskRunner : IStartupTaskRunner
	{
		private readonly IPlayerRepository _playerRepository;
		private readonly IPositionRepository _positionRepository;

		public StartupTaskRunner(IPlayerRepository playerRepository, IPositionRepository positionRepository)
		{
			_playerRepository = playerRepository;
			_positionRepository = positionRepository;
		}

		public async Task ExecuteAsync()
		{
			await CreateInitialPlayers().ConfigureAwait(false);
			await CreateInitialPositions().ConfigureAwait(false);
		}

		private async Task CreateInitialPlayers()
		{
			await _playerRepository.AddPlayerAsync(new Player { Id = 1, Name = "Jools" }).ConfigureAwait(false);
			await _playerRepository.AddPlayerAsync(new Player { Id = 2, Name = "Jops" }).ConfigureAwait(false);
		}

		private async Task CreateInitialPositions()
		{
			await _positionRepository.StorePositionAsync(1, 3, 7).ConfigureAwait(false);
			await _positionRepository.StorePositionAsync(2, 8, 4).ConfigureAwait(false);
		}
	}
}