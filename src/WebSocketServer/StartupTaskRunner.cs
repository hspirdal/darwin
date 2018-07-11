using System.Threading.Tasks;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Identities;
using GameLib.Players;

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
      await _playerRepository.AddorUpdateAsync(new Player { Id = "1", Name = "Jools", GameState = GameState.lobby }).ConfigureAwait(false);
      await _playerRepository.AddorUpdateAsync(new Player { Id = "2", Name = "Jops", GameState = GameState.lobby }).ConfigureAwait(false);
    }

    private async Task CreateInitialIndentities()
    {
      await _identityRepository.AddAsync(new Identity { Id = "1", UserName = "arch", Password = "1234" }).ConfigureAwait(false);
      await _identityRepository.AddAsync(new Identity { Id = "2", UserName = "clip", Password = "1234" }).ConfigureAwait(false);
    }
  }
}