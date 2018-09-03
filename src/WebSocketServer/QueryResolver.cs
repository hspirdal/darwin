using System;
using System.Threading.Tasks;
using Client.Contracts;
using GameLib.Users;
using Newtonsoft.Json;

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
		public QueryResolver(IUserRepository userRepository)
		{
			_userRepository = userRepository;

		}

		public async Task<ServerResponse> ResolveAsync(ClientRequest clientRequest)
		{
			if (clientRequest.RequestName == QueryGetUserState)
			{
				var user = await _userRepository.GetByIdOrDefaultAsync(clientRequest.UserId).ConfigureAwait(false);
				if (user != null)
				{
					var json = JsonConvert.SerializeObject(user.GameState.ToString());
					var response = new ServerResponse { Type = "QueryResponse", Message = QueryGetUserState, Payload = json };
					return response;
				}

				throw new ArgumentException($"User with id '{clientRequest.UserId}' was not found");
			}

			throw new ArgumentException($"Query with name '{clientRequest.RequestName}' is not a valid query command");
		}
	}
}