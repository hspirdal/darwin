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
		private readonly IPremadeCharacterSpawner _premadeCharacterSpawner;

		public LobbyRouter(IUserRepository userRepository, IPremadeCharacterSpawner premadeCharacterSpawner)
		{
			_userRepository = userRepository;
			_premadeCharacterSpawner = premadeCharacterSpawner;
		}

		public async Task RouteAsync(string userId, ClientRequest clientRequest)
		{
			// Temp until there are more actions here.
			if (clientRequest.RequestName == "lobby.newgame")
			{
				Console.WriteLine("Spawning player..");
				_premadeCharacterSpawner.Spawn(userId, clientRequest.Payload);
				await _userRepository.AddOrUpdateAsync(new User { Id = userId, GameState = GameState.InGame }).ConfigureAwait(false);
			}
		}
	}
}