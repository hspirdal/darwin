using System;
using GameLib.Area;
using GameLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLib.Tests.Area
{
	[TestClass]
	public class Map_RemoveEntity
	{
		[TestMethod]
		public void WhenRemovingEntityFromCell_ThenThatCellNoLongerContainsTheEntity()
		{
			var entity = new Entity();
			var map = new TestMap(1, 1);
			map.Add(0, 0, entity);

			var preCount = map.GetCell(0, 0).Content.Entities.Count;
			map.Remove(0, 0, entity);
			var postCount = map.GetCell(0, 0).Content.Entities.Count;

			Assert.AreEqual(1, preCount);
			Assert.AreEqual(0, postCount);
		}

		[TestMethod]
		public void WhenRemovingEntityThatDoesNotExistFromCell_ThenRemoveThrowsArgumentException()
		{
			var entity = new Entity();
			var map = new TestMap(1, 1);

			Assert.ThrowsException<ArgumentException>(() => map.Remove(0, 0, entity));
		}
	}
}