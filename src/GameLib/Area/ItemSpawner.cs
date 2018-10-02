using System;
using GameLib.Actions.Consume;
using GameLib.Entities;

namespace GameLib.Area
{
	public interface IItemSpawner
	{
		void AddRandomly(int totalItemsToAdd);
		void Add(Item item, int x, int y);
	}

	public class ItemSpawner : IItemSpawner
	{
		private readonly IPlayArea _playArea;
		private readonly IPotionFactory _potionFactory;

		public ItemSpawner(IPlayArea playArea, IPotionFactory potionFactory)
		{
			_playArea = playArea;
			_potionFactory = potionFactory;
		}

		public void AddRandomly(int totalItemsToAdd)
		{
			for (var i = 0; i < totalItemsToAdd; ++i)
			{
				var torch = new Item { Name = "Torch", Id = Guid.NewGuid().ToString() };
				var shovel = new Item { Name = "Shovel", Id = Guid.NewGuid().ToString() };
				var barrel = new Container { Name = "Barrel", Id = Guid.NewGuid().ToString() };
				var healingPotion = _potionFactory.CreateByName("Small Healing Potion");
				var cell = _playArea.GameMap.GetRandomOpenCell();
				_playArea.GameMap.Add(cell.X, cell.Y, torch);
				_playArea.GameMap.Add(cell.X, cell.Y, shovel);
				if (i % 2 == 0)
				{
					_playArea.GameMap.Add(cell.X, cell.Y, barrel);
				}
				else
				{
					_playArea.GameMap.Add(cell.X, cell.Y, healingPotion);
				}
			}
		}

		public void Add(Item item, int x, int y)
		{
			_playArea.GameMap.Add(x, y, item);
		}
	}
}