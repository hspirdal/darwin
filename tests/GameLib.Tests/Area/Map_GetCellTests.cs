using System.Linq;
using Autofac.Extras.Moq;
using GameLib.Area;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Area
{
	[TestClass]
	public class Map_GetCellTests
	{
		private Map _map;
		private Entity _entity;

		[TestInitialize]
		public void GivenAMapWithOneCellHavingOneEntity()
		{
			_map = new TestMap(1, 1);

			_entity = new Entity { Name = "Bob", Type = "Player" };
			_map.Add(0, 0, _entity);
		}

		[TestMethod]
		public void WhenGettingCell_ThenCellContainsTheEntity()
		{
			var cell = _map.GetCell(0, 0);
			var entity = cell.Content.Entities.FirstOrDefault();

			Assert.AreSame(_entity, entity);
		}
	}
}