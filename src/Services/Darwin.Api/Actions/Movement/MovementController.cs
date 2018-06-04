using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Darwin.Api.Actions;
using Darwin.Api.Actions.Movement;
using Microsoft.AspNetCore.Mvc;

namespace Darwin.Api.Action.Movement
{
	[Route("api/action/[controller]")]
	public class MovementController : Controller
	{
		private readonly IActionRepository _actionRepository;

		public MovementController(IActionRepository actionRepository)
		{
			_actionRepository = actionRepository;
		}

		// POST api/action/movement
		[HttpPost]
		public async Task<ActionResponse> Post([FromBody]MovementRequest request)
		{
			var successfullyAddded = false;
			var ableToAdd = await _actionRepository.AbleToAddAsync(request.playerId).ConfigureAwait(false);
			if (ableToAdd)
			{
				var movementAction = new MovementAction(request.playerId, request.Direction);
				await _actionRepository.AddAsync(movementAction).ConfigureAwait(false);
				successfullyAddded = true;
			}

			var nextResolveTime = await _actionRepository.GetNextResolveTimeAsync().ConfigureAwait(false);
			return new ActionResponse
			{
				ActionQueued = successfullyAddded,
				CurrentActionQueued = $"Movement.{request.Direction}",
				NextResolveTime = nextResolveTime,
				CurrentTime = DateTime.UtcNow
			};
		}
	}
}
