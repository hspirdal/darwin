using System.Linq;
using System.Threading.Tasks;
using GameLib.Logging;

namespace GameLib.Identities
{
    public interface IAuthenticator
    {
        Task<Identity> AuthenticateAsync(AuthentificationRequest AuthentificationRequest);

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

        public Task<Identity> AuthenticateAsync(AuthentificationRequest request)
        {
            return Task.Run(() =>
            {
                var identities = _identityRepository.GetAll();
                var identity = identities.SingleOrDefault(i => i.UserName == request.UserName && i.Password == request.Password);

                if (identity == null)
                {
                    _logger.Warn($"Failed to authenticate user (connectionId: {request.ConnectionId}, username: {request.UserName}, " +
                        $"password:{request.Password})");
                }

                return identity;
            });
        }
    }
}