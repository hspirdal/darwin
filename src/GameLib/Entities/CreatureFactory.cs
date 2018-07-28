using System;
using System.Collections.Generic;
using GameLib.Utility;

namespace GameLib.Entities
{
	public interface ICreatureFactory
	{
		Creature CreateRandom();
	}

	public class CreatureFactory : ICreatureFactory
	{
		private readonly List<Creature> _creatureTemplates;
		private readonly IRandomNumberGenerator _randomGenerator;

		public CreatureFactory(List<Creature> creatureTemplates, IRandomNumberGenerator randomGenerator)
		{
			_creatureTemplates = creatureTemplates;
			_randomGenerator = randomGenerator;
		}

		public Creature CreateRandom()
		{
			var index = _randomGenerator.Next(0, _creatureTemplates.Count);
			var creature = _creatureTemplates[index].DeepCopy();
			creature.Id = Guid.NewGuid().ToString();
			return creature;
		}
	}
}