using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Darwin.Api.Identity
{
	[Route("api/[controller]")]
	public class PlayerController
	{
		private readonly IPlayerRepository _playerRepository;

		public PlayerController(IPlayerRepository playerRepository)
		{
			_playerRepository = playerRepository;
		}

		// GET api/player/5
		[HttpGet("{id}")]
		public async Task<PlayerResponse> GetAsync(int id)
		{
			var player = await _playerRepository.GetByIdAsync(id);

			return new PlayerResponse
			{
				Id = player.Id,
				Name = player.Name
			};
		}
	}
}