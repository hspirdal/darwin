using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class InventoryProfileTests
	{
		[TestMethod]
		public void WhenMappingInventory_ThenMappingIsSuccessfull()
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(InventoryProfile));
			});

			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}