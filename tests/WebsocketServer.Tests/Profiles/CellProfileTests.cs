using System;
using System.Collections.Generic;
using AutoMapper;
using GameLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class CellProfileTests
	{
		private MapperConfiguration _mapperConfiguration;

		[TestInitialize]
		public void GivenAValidCellProfile()
		{
			_mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(CreatureProfile));
				cfg.AddProfile(typeof(CellProfile));
			});
		}

		[TestMethod]
		public void WhenMappingCell_ThenMappingIsSuccessfull()
		{
			_mapperConfiguration.AssertConfigurationIsValid();
		}

		[TestMethod]
		public void WhenMappingCell_ThenCreatureCountIsCorrect()
		{
			var creatures = new List<GameLib.Entities.Entity> { new GameLib.Entities.Creature() };
			var cellContent = new GameLib.Area.CellContent { Entities = creatures };
			var cell = new GameLib.Area.Cell { Content = cellContent };
			var mapper = _mapperConfiguration.CreateMapper();

			var mappedCell = mapper.Map<GameLib.Area.Cell, Client.Contracts.Area.Cell>(cell);

			Assert.IsTrue(mappedCell.Creatures.Count == 1);
		}
	}
}