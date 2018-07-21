using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class CreatureProfileTests
	{
		[TestMethod]
		public void WhenMappingPlayer_ThenMappingIsSuccessfull()
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(CreatureProfile));
			});

			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}