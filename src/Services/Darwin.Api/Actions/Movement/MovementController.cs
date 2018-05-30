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
		// POST api/action/movement
		[HttpPost]
		public ActionResponse Post([FromBody]MovementRequest movementRequest)
		{
			return new ActionResponse
			{
				ActionQueued = true,
				CurrentActionQueued = $"Movement.{movementRequest.Direction}",
				NextActionAvailable = DateTime.UtcNow.AddSeconds(1)
			};
		}
	}
}
