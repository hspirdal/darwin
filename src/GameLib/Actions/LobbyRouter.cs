using System;
using System.Threading.Tasks;
using GameLib.Area;
using GameLib.Entities;
using GameLib.Properties;
using Client.Contracts;
using GameLib.Identities;
using GameLib.Users;

namespace GameLib.Actions
{
	public interface ILobbyRouter : IRouter { }

	public class LobbyRouter : ILobbyRouter
	{
		private readonly IUserRepository _userRepository;
		private readonly IPlayArea _playArea;

		public LobbyRouter(IUserRepository userRepository, IPlayArea playArea)
		{
			_userRepository = userRepository;
			_playArea = playArea;
		}

		public async Task RouteAsync(string userId, ClientRequest clientRequest)
		{
			// Temp until there are more actions here.
			if (clientRequest.RequestName == "lobby.newgame")
			{
				Console.WriteLine("Spawning player..");
				var user = await _userRepository.GetByIdAsync(userId).ConfigureAwait(false);
				user.GameState = GameState.InGame;
				await _userRepository.AddOrUpdateAsync(user).ConfigureAwait(false);
			}
		}
	}
}