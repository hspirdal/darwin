using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Identities;
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

		public AccountController(IIdentityRepository identityRepository, ILogger<AccountController> logger)
		{
			_identityRepository = identityRepository;
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

				return new LoginResponse
				{
					Success = true,
					SessionId = sessionId
				};
			}

			_logger.LogWarning($"Failed to authenticate");
			return new LoginResponse
			{
				Success = false,
			};
		}
	}
}