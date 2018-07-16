using System.Linq;
using GameLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLib.Tests.Area
{
	[TestClass]
	public class Map_AddEntity
	{
		[TestMethod]
		public void WhenAddingEntityToCell_ThenEntityIsContainedWhenGettingCell()
		{
			var entity = new Entity();
			var map = new TestMap(1, 1);
			map.Add(0, 0, entity);

			var cellEntity = map.GetCell(0, 0).Content.Entities.Find(i => i.Equals(entity));

			Assert.AreSame(entity, cellEntity);
		}
	}
}