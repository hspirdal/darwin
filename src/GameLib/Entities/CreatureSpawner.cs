using GameLib.Area;

namespace GameLib.Entities
{
	public interface ICreatureSpawner
	{
		void SpawnRandomly(int creatureCount);
	}
	public class CreatureSpawner : ICreatureSpawner
	{
		private readonly IPlayArea _playArea;
		private readonly ICreatureFactory _creatureFactory;
		private readonly ICreatureRegistry _creatureRegistry;

		public CreatureSpawner(IPlayArea playArea, ICreatureFactory creatureFactory, ICreatureRegistry creatureRegistry)
		{
			_playArea = playArea;
			_creatureFactory = creatureFactory;
			_creatureRegistry = creatureRegistry;
		}

		public void SpawnRandomly(int creatureCount)
		{
			for (var i = 0; i < creatureCount; ++i)
			{
				var creature = _creatureFactory.CreateRandom();
				var openCell = _playArea.GameMap.GetRandomOpenCell();
				_playArea.GameMap.Add(openCell.X, openCell.Y, creature);
				_creatureRegistry.Register(creature);
			}
		}
	}
}