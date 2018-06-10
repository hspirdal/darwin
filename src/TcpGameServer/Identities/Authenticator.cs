using System.Linq;
using System.Threading.Tasks;
using TcpGameServer.Logging;

namespace TcpGameServer.Identities
{
	public interface IAuthenticator
	{
		Identity Authenticate(AuthentificationRequest AuthentificationRequest);

	}

	public class Authenticator : IAuthenticator
	{
		private readonly IIdentityRepository _identityRepository;
		private readonly ILogger _logger;
		public Authenticator(ILogger logger, IIdentityRepository identityRepository)
		{
			_logger = logger;
			_identityRepository = identityRepository;

		}

		public Identity Authenticate(AuthentificationRequest request)
		{
			var identities = _identityRepository.GetAll();
			var identity = identities.SingleOrDefault(i => i.UserName == request.UserName && i.Password == request.Password);

			if (identity == null)
			{
				_logger.Warn($"Failed to authenticate user (connectionId: {request.ConnectionId}, username: {request.UserName}, " +
					$"password:{request.Password})");
			}

			return identity;
		}
	}
}