using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class ProfileTests
	{
		[TestMethod]
		public void WhenMappingCell_ThenMappingIsSuccessfull()
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(CellProfile));
			});

			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}