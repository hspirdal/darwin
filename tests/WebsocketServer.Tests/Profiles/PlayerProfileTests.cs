using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class PlayerProfileTests
	{
		[TestMethod]
		public void WhenMappingPlayer_ThenMappingIsSuccessfull()
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(PlayerProfile));
			});

			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}