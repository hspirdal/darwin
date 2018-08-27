using GameLib.Entities;
using GameLib.Properties;
using System;
using System.Threading.Tasks;
using Client.Contracts;
using GameLib.Users;

namespace GameLib.Actions
{
	public interface IStateRequestRouter : IRouter { }

	public class StateRequestRouter : IStateRequestRouter
	{
		private readonly ILobbyRouter _lobbyRouter;
		private readonly IGameRouter _gameRouter;
		private readonly IUserRepository _userRepository;

		public StateRequestRouter(ILobbyRouter lobbyRouter, IGameRouter gameRouter, IUserRepository userRepository)
		{
			_lobbyRouter = lobbyRouter;
			_gameRouter = gameRouter;
			_userRepository = userRepository;
		}

		public async Task RouteAsync(string userId, ClientRequest clientRequest)
		{
			// TODO: Create user object at successful login instead..
			var userCreated = await _userRepository.ContainsAsync(userId).ConfigureAwait(false);
			if (!userCreated)
			{
				await _userRepository.AddOrUpdateAsync(new User { Id = userId, GameState = GameState.lobby }).ConfigureAwait(false);
			}

			var user = await _userRepository.GetByIdAsync(userId).ConfigureAwait(false);
			if (user.GameState == GameState.lobby)
			{
				await _lobbyRouter.RouteAsync(userId, clientRequest);
			}
			else
			{
				await _gameRouter.RouteAsync(userId, clientRequest);
			}
		}
	}
}