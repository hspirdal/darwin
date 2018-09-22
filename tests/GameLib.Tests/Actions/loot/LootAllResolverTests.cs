using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using GameLib.Actions.Loot;
using GameLib.Area;
using GameLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Actions.loot
{
	[TestClass]
	public class LootAllResolverTests
	{
		private AutoMock _container;
		private LootAllResolver _resolver;
		private Player _player;
		private Mock<ICreatureRegistry> _creatureRegistry;
		private Mock<IPlayArea> _playArea;
		private Item _itemOne;
		private Item _itemTwo;
		private Cell _cell;

		[TestInitialize]
		public void Arrange()
		{
			_container = AutoMock.GetLoose();
			_resolver = _container.Create<LootAllResolver>();
			_player = new Player { Id = "arbitraryId" };

			_playArea = _container.Mock<IPlayArea>();
			_creatureRegistry = _container.Mock<ICreatureRegistry>();
			_creatureRegistry.Setup(i => i.GetById(It.IsAny<string>())).Returns(_player);

			_itemOne = new Item("some_id", "Torch", SubType.None);
			_itemTwo = new Item("some_other_id", "Broadsword", SubType.Weapon);

			_cell = new Cell { Content = new CellContent { Entities = new List<Entity> { _itemOne, _itemTwo } } };
			_playArea.Setup(i => i.GameMap.GetCell(It.IsAny<int>(), It.IsAny<int>())).Returns(_cell);
		}

		[TestMethod]
		public async Task WhenAPlayerLootsAllItems_ThenThoseAreMovedToInventory()
		{
			var action = new LootAllAction(_player.Id);

			await _resolver.ResolveAsync(action);

			var inventoryItems = _player.Inventory.Items;
			Assert.AreEqual(2, inventoryItems.Count);
			Assert.AreEqual(0, _cell.Content.Entities.Count);
		}
	}
}