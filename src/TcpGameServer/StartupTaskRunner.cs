using System.Threading.Tasks;
using TcpGameServer.Actions;
using TcpGameServer.Actions.Movement;
using TcpGameServer.Identities;
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
        private readonly IIdentityRepository _identityRepository;

        public StartupTaskRunner(IPlayerRepository playerRepository, IPositionRepository positionRepository, IIdentityRepository identityRepository)
        {
            _playerRepository = playerRepository;
            _positionRepository = positionRepository;
            _identityRepository = identityRepository;
        }

        public async Task ExecuteAsync()
        {
            await CreateInitialIndentities().ConfigureAwait(false);
            await CreateInitialPlayers().ConfigureAwait(false);
            await CreateInitialPositions().ConfigureAwait(false);
        }

        private async Task CreateInitialPlayers()
        {
            await _playerRepository.AddPlayerAsync(new Player { Id = 1, Name = "Jools", GameState = GameState.lobby }).ConfigureAwait(false);
            await _playerRepository.AddPlayerAsync(new Player { Id = 2, Name = "Jops", GameState = GameState.lobby }).ConfigureAwait(false);
        }

        private async Task CreateInitialPositions()
        {
            await _positionRepository.StorePositionAsync("1", 3, 7).ConfigureAwait(false);
            await _positionRepository.StorePositionAsync("2", 8, 4).ConfigureAwait(false);
        }

        private async Task CreateInitialIndentities()
        {
            await _identityRepository.AddAsync(new Identity { Id = "1", UserName = "arch", Password = "1234" }).ConfigureAwait(false);
            await _identityRepository.AddAsync(new Identity { Id = "2", UserName = "clip", Password = "1234" }).ConfigureAwait(false);
        }
    }
}