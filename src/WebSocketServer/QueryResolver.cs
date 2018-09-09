using System;
using System.Threading.Tasks;
using Client.Contracts;
using GameLib.Properties;
using GameLib.Users;

namespace WebSocketServer
{
	public interface IQueryResolver
	{
		Task<ServerResponse> ResolveAsync(ClientRequest clientRequest);
	}
	public class QueryResolver : IQueryResolver
	{
		private readonly IUserRepository _userRepository;
		private const string QueryGetUserState = "Query.GetUserState";
		private const string QueryForfeitGame = "Query.ForfeitGame";

		public QueryResolver(IUserRepository userRepository)
		{
			_userRepository = userRepository;

		}

		public async Task<ServerResponse> ResolveAsync(ClientRequest clientRequest)
		{
			if (clientRequest.RequestName == QueryGetUserState)
			{
				var user = await _userRepository.GetByIdOrDefaultAsync(clientRequest.UserId).ConfigureAwait(false);
				EnsureValidUser(clientRequest.UserId, user);

				return new ServerResponse(ResponseType.GameState, user.GameState.ToString());
			}
			else if (clientRequest.RequestName == QueryForfeitGame)
			{
				// TODO: Handle this better in the future, currently restrict to only move state from game-over to lobby
				var user = await _userRepository.GetByIdOrDefaultAsync(clientRequest.UserId).ConfigureAwait(false);
				EnsureValidUser(clientRequest.UserId, user);

				if (user.GameState == GameState.PlayerDeath)
				{
					user.GameState = GameState.GameLobby;
					await _userRepository.AddOrUpdateAsync(user).ConfigureAwait(false);
					return new ServerResponse(ResponseType.GameState, user.GameState.ToString());
				}

				return new ServerResponse(ResponseType.RequestDeclined, "Only able to forfeit game when player is dead.");
			}

			throw new ArgumentException($"Query with name '{clientRequest.RequestName}' is not a valid query command");
		}

		private void EnsureValidUser(string userId, User user)
		{
			if (user == null)
			{
				throw new ArgumentException($"User with id '{userId}' was not found");
			}
		}
	}
}