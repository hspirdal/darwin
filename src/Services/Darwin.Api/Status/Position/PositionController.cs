using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Darwin.Api.Status.Position
{
	[Route("api/status/[controller]")]
	public class PositionController : Controller
	{
		// GET api/status/position
		[HttpGet]
		public IEnumerable<PositionResponse> Get()
		{
			return new PositionResponse[]
			{
				new PositionResponse
				{
					PlayerId = 3,
					X = 5,
					Y = 7
				},
				new PositionResponse
				{
					PlayerId = 5,
					X = 2,
					Y = 2
				},
			};
		}

		// GET api/status/position/5
		[HttpGet("{id}")]
		public PositionResponse Get(int id)
		{
			return new PositionResponse
			{
				PlayerId = 5,
				X = 2,
				Y = 2
			};
		}
	}
}
