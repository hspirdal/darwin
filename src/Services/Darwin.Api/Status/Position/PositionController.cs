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
		public async Task<IEnumerable<PositionResponse>> GetAsync()
		{
			var positions = await _positionRepository.GetAllAsync().ConfigureAwait(false);

			var response = new List<PositionResponse>();
			foreach (var p in positions)
			{
				response.Add(CreateResponse(p.OwnerId, p.Position));
			}

			return response;
		}

		// GET api/status/position/5
		[HttpGet("{id}")]
		public async Task<PositionResponse> GetAsync(int id)
		{
			var position = await _positionRepository.GetByIdAsync(id).ConfigureAwait(false);
			return CreateResponse(id, position);
		}

		private static PositionResponse CreateResponse(int id, Position position)
		{
			return new PositionResponse
			{
				PlayerId = id,
				X = position.X,
				Y = position.Y
			};
		}
	}
}
