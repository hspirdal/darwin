using System;
using System.Collections.Generic;
using System.Linq;
using GameLib.Entities;
using GameLib.Properties.Stats;
using GameLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Entities
{
	[TestClass]
	public class CreatureFactory_CreateRandom
	{
		private List<Creature> _templates;
		private Mock<IRandomGenerator> _randomGenerator;
		private ICreatureFactory _factory;

		[TestInitialize]
		public void GivenAFactoryWithSomeCreatureTemplates()
		{
			_templates = new List<Creature>
			{
				CreateMockCreature("Goblin"),
				CreateMockCreature("Orc"),
				CreateMockCreature("Bat"),
				CreateMockCreature("Wolf"),
			};

			_randomGenerator = new Mock<IRandomGenerator>();

			_factory = new CreatureFactory(_templates, _randomGenerator.Object);
		}

		[TestMethod]
		public void WhenCreatingARandomCreatureFromTemplate_ThenIdIsDifferent()
		{
			var templateCreature = _templates[0];
			_randomGenerator.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(0);

			var newCreature = _factory.CreateRandom();

			Assert.AreNotEqual(templateCreature.Id, newCreature.Id);
		}

		[TestMethod]
		public void WhenRandomRollsSpecificIndex_ThenCreatureOfThatIndexIsCreated()
		{
			_randomGenerator.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(2);

			var creature = _factory.CreateRandom();

			Assert.AreEqual("Bat", creature.Name);
		}
		private Creature CreateMockCreature(string name)
		{
			return new Creature(Guid.NewGuid().ToString(), name, "MockRace", "MockClass", new Statistics());
		}
	}
}