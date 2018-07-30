using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class CreatureProfileTests
	{
		private MapperConfiguration _mapperConfiguration;

		[TestInitialize]
		public void GivenAValidCreatureProfile()
		{
			_mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(CreatureProfile));
			});
		}

		[TestMethod]
		public void WhenMappingCreature_ThenMappingIsSuccessfull()
		{
			_mapperConfiguration.AssertConfigurationIsValid();
		}

		[TestMethod]
		public void WhenMappingCreature_ThenLevelFieldIsMappedCorrectly()
		{
			var creature = new GameLib.Entities.Creature() { Race = "Elf" };
			var mapper = _mapperConfiguration.CreateMapper();

			var mappedPlayer = mapper.Map<GameLib.Entities.Creature, Client.Contracts.Entities.Creature>(creature);

			Assert.AreEqual("Elf", mappedPlayer.Race);
		}
	}
}