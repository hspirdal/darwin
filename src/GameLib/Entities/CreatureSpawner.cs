using GameLib.Area;
using GameLib.Messaging;
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
		private readonly IRecipientRegistry _recipientRegistry;

		public CreatureSpawner(IPlayArea playArea, ICreatureFactory creatureFactory, ICreatureRegistry creatureRegistry,
			IAutonomousFactory autonomousFactory, IAutonomousRegistry autonomousRegistry, IRecipientRegistry recipientRegistry)
		{
			_playArea = playArea;
			_creatureFactory = creatureFactory;
			_creatureRegistry = creatureRegistry;
			_autonomousFactory = autonomousFactory;
			_autonomousRegistry = autonomousRegistry;
			_recipientRegistry = recipientRegistry;
		}

		public void SpawnRandomly(int creatureCount)
		{
			for (var i = 0; i < creatureCount; ++i)
			{
				var creature = _creatureFactory.CreateRandom();
				var autonomousModel = _autonomousFactory.Create("simpledefender", creature);
				var openCell = _playArea.GameMap.GetRandomOpenCell();

				// TODO: Simplify all the registering.
				_playArea.GameMap.Add(openCell.X, openCell.Y, creature);
				creature.Position.SetPosition(openCell.X, openCell.Y);
				_autonomousRegistry.Register(autonomousModel);
				_creatureRegistry.Register(creature);
				_recipientRegistry.Register(autonomousModel);
			}
		}
	}
}