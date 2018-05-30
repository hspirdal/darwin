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
		private readonly IPositionRepository _positionRepository;

		public PositionController(IPositionRepository positionRepository)
		{
			_positionRepository = positionRepository;
		}

		// GET api/status/position
		[HttpGet]
		public IEnumerable<PositionResponse> Get()
		{
			var positionMap = _positionRepository.GetAll();
			var positions = new List<PositionResponse>();
			foreach (var kvp in positionMap)
			{
				positions.Add(new PositionResponse
				{
					PlayerId = kvp.Key,
					X = kvp.Value.X,
					Y = kvp.Value.Y
				});
			}

			return positions;
		}

		// GET api/status/position/5
		[HttpGet("{id}")]
		public PositionResponse Get(int id)
		{
			var position = _positionRepository.GetById(id);
			return new PositionResponse
			{
				PlayerId = id,
				X = position.X,
				Y = position.Y
			};
		}
	}
}
