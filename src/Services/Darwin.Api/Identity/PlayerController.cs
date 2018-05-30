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
		public PlayerResponse Get(int id)
		{
			var player = _playerRepository.GetById(id);

			return new PlayerResponse
			{
				Id = player.Id,
				Name = player.Name
			};
		}
	}
}