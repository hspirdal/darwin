using System;
using System.Collections.Generic;

namespace GameLib.Entities
{
	public interface ICreatureFactory
	{
		Creature CreateRandom();
	}

	public class CreatureFactory : ICreatureFactory
	{
		private readonly List<Creature> _creatureTemplates;
		private readonly Random _random;

		public CreatureFactory(List<Creature> creatureTemplates)
		{
			_creatureTemplates = creatureTemplates;
			_random = new Random();
		}

		public Creature CreateRandom()
		{
			var index = _random.Next(0, _creatureTemplates.Count - 1);
			var creature = _creatureTemplates[index].DeepCopy();
			creature.Id = Guid.NewGuid().ToString();
			return creature;
		}
	}
}