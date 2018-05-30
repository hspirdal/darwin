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
		private readonly IMovementResolver _movementResolver;

		public MovementController(IActionRepository actionRepository, IMovementResolver movementResolver)
		{
			_actionRepository = actionRepository;
			_movementResolver = movementResolver;
		}

		// POST api/action/movement
		[HttpPost]
		public ActionResponse Post([FromBody]MovementRequest request)
		{
			var successfullyAddded = false;
			if (_actionRepository.AbleToAdd(request.playerId))
			{
				var movementAction = new MovementAction(_movementResolver, request.playerId, request.Direction);
				_actionRepository.Add(movementAction);
				successfullyAddded = true;
			}


			return new ActionResponse
			{
				ActionQueued = successfullyAddded,
				CurrentActionQueued = $"Movement.{request.Direction}",
				NextActionAvailable = _actionRepository.NextActionAvailable
			};
		}
	}
}
