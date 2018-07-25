using System;
using Autofac.Extras.Moq;
using GameLib.Actions;
using GameLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLib.Tests.Actions
{
	[TestClass]
	public class CooldownRegistryTests
	{
		private AutoMock _container;
		private CooldownRegistry _registry;

		[TestInitialize]
		public void GivenAValidCooldownRegistry()
		{
			_container = AutoMock.GetLoose();
			_registry = _container.Create<CooldownRegistry>();
		}

		[TestMethod]
		public void WhenAddingCooldownToRegistry_ThenRegistryCountIncreasesByOne()
		{

			var cooldownToAdd = new Cooldown("arbitraryId", DateTime.MinValue);

			var preCount = _registry.Count;
			_registry.Add(cooldownToAdd);
			var postCount = _registry.Count;

			Assert.AreEqual(preCount + 1, postCount);
		}

		[TestMethod]
		public void WhenPreviousCooldownIsNotValidAnymoreWhileAdding_ThenOldCooldownIsReplaced()
		{
			var validToDate = DateTime.UtcNow + TimeSpan.FromSeconds(5);
			var initialCooldown = new Cooldown("arbitraryId", validToDate);
			var cooldownToAdd = new Cooldown("arbitraryId", DateTime.MaxValue);
			var initialCooldownAddedSuccessfully = _registry.Add(initialCooldown);
			_container.Mock<IClock>().Setup(i => i.UtcNow).Returns(validToDate);

			var cooldownReplacedOldOneSuccessfully = _registry.Add(cooldownToAdd);

			Assert.IsTrue(initialCooldownAddedSuccessfully);
			Assert.IsTrue(cooldownReplacedOldOneSuccessfully);
		}

		[TestMethod]
		public void WhenCooldownValidToTimeIsOver_ThenCooldownIsNotInEffectAnymore()
		{
			var validToDate = DateTime.UtcNow + TimeSpan.FromSeconds(5);
			var initialCooldown = new Cooldown("arbitraryId", validToDate);
			var initialCooldownAddedSuccessfully = _registry.Add(initialCooldown);
			_container.Mock<IClock>().Setup(i => i.UtcNow).Returns(validToDate);

			var isCooldownInEffect = _registry.IsCooldownInEffect("arbitraryId");

			Assert.IsTrue(initialCooldownAddedSuccessfully);
			Assert.IsFalse(isCooldownInEffect);
		}

		[TestMethod]
		public void WhenAddingCooldownWhenAnotherOneIsAlreadyInEffect_ThenRegistryReturnFalse()
		{
			var initialCooldown = new Cooldown("arbitraryId", DateTime.MaxValue);
			var cooldownToAdd = new Cooldown("arbitraryId", DateTime.MinValue);
			_container.Mock<IClock>().Setup(i => i.UtcNow).Returns(DateTime.UtcNow);
			_registry.Add(initialCooldown);

			var result = _registry.Add(cooldownToAdd);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void WhenAddingCooldownWithInvalidUserId_ThenRegistryThrowsArgumentException()
		{
			var invalidUserId = string.Empty;
			var cooldownToAdd = new Cooldown(invalidUserId, DateTime.MinValue);
			Assert.ThrowsException<ArgumentException>(() => _registry.Add(cooldownToAdd));
		}
	}
}