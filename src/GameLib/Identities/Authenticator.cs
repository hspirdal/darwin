using System;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Logging;
using GameLib.Users;

namespace GameLib.Identities
{
	public interface IAuthenticator
	{
		Task<bool> AuthenticateAsync(string userId, Guid sessionId);
	}

	public class Authenticator : IAuthenticator
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger _logger;
		public Authenticator(ILogger logger, IUserRepository userRepository)
		{
			_logger = logger;
			_userRepository = userRepository;

		}

		public async Task<bool> AuthenticateAsync(string userId, Guid sessionId)
		{
			var user = await _userRepository.GetByIdOrDefaultAsync(userId).ConfigureAwait(false);
			if (user == null || user.SessionId != sessionId)
			{
				_logger.Warn($"Failed to authenticate user with id '{userId}' and sessionId '{sessionId.ToString()}'");
				return false;
			}

			return true;
		}
	}
}