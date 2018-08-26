using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Identities;
using GameLib.Properties;
using GameLib.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Darwin.Api.Identity
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IIdentityRepository _identityRepository;
		private readonly ILogger<AccountController> _logger;
		private readonly IUserRepository _userRepository;

		public AccountController(IIdentityRepository identityRepository, IUserRepository userRepository, ILogger<AccountController> logger)
		{
			_identityRepository = identityRepository;
			_userRepository = userRepository;
			_logger = logger;
		}

		[HttpPost]
		[ProducesResponseType(400)]
		public async Task<ActionResult<LoginResponse>> LoginAsync(IdentityModel model)
		{
			var identities = await _identityRepository.GetAllAsync().ConfigureAwait(false);
			var identity = identities.SingleOrDefault(i => i.UserName == model.UserName);
			if (identity != null && identity.Password == model.Password)
			{
				var sessionId = Guid.NewGuid();
				identity.SessionId = sessionId;

				await _identityRepository.AddOrUpdateAsync(identity).ConfigureAwait(false);
				var user = await RetrieveUserAsync(identity.Id).ConfigureAwait(false);
				user.SessionId = identity.SessionId;
				await _userRepository.AddOrUpdateAsync(user).ConfigureAwait(false);

				return new LoginResponse
				{
					Success = true,
					UserId = identity.Id,
					SessionId = sessionId
				};
			}

			_logger.LogWarning($"Failed to authenticate");
			return new LoginResponse
			{
				Success = false,
			};
		}

		private async Task<User> RetrieveUserAsync(string id)
		{
			var userExists = await _userRepository.ContainsAsync(id).ConfigureAwait(false);
			if (userExists)
			{
				return await _userRepository.GetByIdAsync(id).ConfigureAwait(false);
			}

			return new User { Id = id, GameState = GameState.lobby };
		}
	}
}