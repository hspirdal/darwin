using GameLib.Area;
using GameLib.Properties.Autonomy;

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
		private readonly IAutonomousFactory _autonomousFactory;
		private readonly IAutonomousRegistry _autonomousRegistry;

		public CreatureSpawner(IPlayArea playArea, ICreatureFactory creatureFactory, ICreatureRegistry creatureRegistry,
			IAutonomousFactory autonomousFactory, IAutonomousRegistry autonomousRegistry)
		{
			_playArea = playArea;
			_creatureFactory = creatureFactory;
			_creatureRegistry = creatureRegistry;
			_autonomousFactory = autonomousFactory;
			_autonomousRegistry = autonomousRegistry;
		}

		public void SpawnRandomly(int creatureCount)
		{
			for (var i = 0; i < creatureCount; ++i)
			{
				var creature = _creatureFactory.CreateRandom();
				var autonomousModel = _autonomousFactory.Create("simpledefender", creature);
				var openCell = _playArea.GameMap.GetRandomOpenCell();
				_playArea.GameMap.Add(openCell.X, openCell.Y, creature);
				creature.Position.SetPosition(openCell.X, openCell.Y);
				_autonomousRegistry.Register(autonomousModel);
				_creatureRegistry.Register(creature);
			}
		}
	}
}