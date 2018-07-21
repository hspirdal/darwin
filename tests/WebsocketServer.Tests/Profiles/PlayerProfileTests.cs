using System;
using AutoMapper;
using GameLib.Properties.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class PlayerProfileTests
	{
		private MapperConfiguration _mapperConfiguration;

		[TestInitialize]
		public void GivenAValidPlayerProfile()
		{
			_mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(PlayerProfile));
			});
		}

		[TestMethod]
		public void WhenMappingPlayer_ThenMappingIsSuccessfull()
		{
			_mapperConfiguration.AssertConfigurationIsValid();
		}

		[TestMethod]
		public void WhenMappingPlayer_ThenLevelFieldIsMappedCorrectly()
		{
			var player = new GameLib.Entities.Player() { Level = new AttributeScore { Base = 8 } };
			var mapper = _mapperConfiguration.CreateMapper();

			var mappedPlayer = mapper.Map<GameLib.Entities.Player, Client.Contracts.Entities.Player>(player);

			Assert.AreEqual(8, mappedPlayer.Level);
		}
	}
}