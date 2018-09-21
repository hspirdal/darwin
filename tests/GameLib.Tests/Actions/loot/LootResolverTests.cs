using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac.Extras.Moq;
using GameLib.Actions.Loot;
using GameLib.Area;
using GameLib.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GameLib.Tests.Actions.loot
{
	[TestClass]
	public class LootResolverTests
	{
		private AutoMock _container;
		private LootResolver _resolver;
		private Player _player;
		private Mock<IPlayArea> _playArea;
		private Mock<ICreatureRegistry> _creatureRegistry;
		private Item _item;
		private Cell _cell;

		[TestInitialize]
		public void Arrange()
		{
			_container = AutoMock.GetLoose();
			_resolver = _container.Create<LootResolver>();
			_player = new Player { Id = "arbitraryId" };

			_playArea = _container.Mock<IPlayArea>();
			_creatureRegistry = _container.Mock<ICreatureRegistry>();
			_creatureRegistry.Setup(i => i.GetById(It.IsAny<string>())).Returns(_player);

			_item = new Item("some_id", "Torch", SubType.None);
			_cell = new Cell { Content = new CellContent { Entities = new List<Entity> { _item } } };
			_playArea.Setup(i => i.GameMap.GetCell(It.IsAny<int>(), It.IsAny<int>())).Returns(_cell);
		}

		[TestMethod]
		public async Task WhenAPlayerLootsAValidItem_ThenThatItemAppearsIntoPlayerInventory()
		{
			var action = new LootAction(_player.Id, _item.Id);

			await _resolver.ResolveAsync(action);

			var inventoryItem = _player.Inventory.Items.First();
			Assert.AreEqual(_item.Id, inventoryItem.Id);
		}

		[TestMethod]
		public async Task WhenAPlayerLootsAValidItem_ThenThatItemObjectReferenceIsMovedToPlayerInventory()
		{
			var action = new LootAction(_player.Id, _item.Id);

			await _resolver.ResolveAsync(action);

			var inventoryItem = _player.Inventory.Items.First();
			Assert.AreSame(_item, inventoryItem);
		}

		[TestMethod]
		public async Task WhenPlayerLootsAValidItem_ThenThatItemIsRemovedFromCell()
		{
			var action = new LootAction(_player.Id, _item.Id);

			var preLootCellCount = _cell.Content.Entities.Count();
			await _resolver.ResolveAsync(action);
			var postLootCellCount = _cell.Content.Entities.Count();

			Assert.AreEqual(preLootCellCount - 1, postLootCellCount);
		}

		[TestMethod]
		public async Task WhenPlayerLootsAnItemThatDoesNotExistInCell_ThenNothingIsAddedToPlayerInventory()
		{
			_playArea.Setup(i => i.GameMap.GetCell(It.IsAny<int>(), It.IsAny<int>())).Returns(new Cell());

			var action = new LootAction(_player.Id, _item.Id);

			var preLootInventoryCount = _player.Inventory.Items.Count();
			await _resolver.ResolveAsync(action);
			var postLootInventoryCount = _player.Inventory.Items.Count();

			Assert.AreEqual(preLootInventoryCount, postLootInventoryCount);
		}
	}
}