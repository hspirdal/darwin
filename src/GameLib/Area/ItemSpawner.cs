using System;

namespace GameLib.Area
{
	public interface IItemSpawner
	{
		void AddRandomly();
	}

	public class ItemSpawner : IItemSpawner
	{
		private readonly IPlayArea _playArea;

		public ItemSpawner(IPlayArea playArea)
		{
			_playArea = playArea;
		}

		public void AddRandomly()
		{
			var torch = new Item { Name = "Torch", Id = Guid.NewGuid().ToString() };
			var cell = _playArea.GameMap.GetRandomOpenCell();
			_playArea.GameMap.Add(cell.X, cell.Y, torch);
		}
	}
}