using System;
using System.Collections.Generic;
using GameLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLib.Tests.Entities
{
	[TestClass]
	public class CreatureFactory_CreateRandom
	{
		private Creature _templateCreature;
		private ICreatureFactory _factory;

		[TestInitialize]
		public void GivenAFactoryWithOneCreatureAsTemplate()
		{
			_templateCreature = new Creature { Id = "some id" };
			_factory = new CreatureFactory(new List<Creature> { _templateCreature });
		}

		[TestMethod]
		public void WhenCreatingARandomCreatureFromTemplate_ThenIdIsDifferent()
		{
			var newCreature = _factory.CreateRandom();

			Assert.AreNotEqual(_templateCreature.Id, newCreature.Id);
		}
	}
}