using GameLib.Properties.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLib.Tests.Properties.Stats
{
	[TestClass]
	public class HealthinessReaderTests
	{
		[TestMethod]
		public void WhenCurrentHitPointsIsAtMax_ThenCreatureLooksHealthy()
		{
			var hitPoints = new HitPoints(100);
			var condition = HealthinessReader.Measure(hitPoints);

			Assert.AreEqual("Healthy", condition);
		}

		[TestMethod]
		public void WhenCurrentHitPointsIsAbove79Percent_ThenCreatureLooksSlightlyInjured()
		{
			var hitPoints = new HitPoints(100) { Current = 80 };
			var condition = HealthinessReader.Measure(hitPoints);

			Assert.AreEqual("Slightly Injured", condition);
		}

		[TestMethod]
		public void WhenCurrentHitPointsIsAbove39Percent_ThenCreatureLooksInjured()
		{
			var hitPoints = new HitPoints(100) { Current = 40 };
			var condition = HealthinessReader.Measure(hitPoints);

			Assert.AreEqual("Injured", condition);
		}

		[TestMethod]
		public void WhenCurrentHitPointsIsAbove14Percent_ThenCreatureLooksBadlyInjured()
		{
			var hitPoints = new HitPoints(100) { Current = 15 };
			var condition = HealthinessReader.Measure(hitPoints);

			Assert.AreEqual("Badly Injured", condition);
		}

		[TestMethod]
		public void WhenCurrentHitPointsIsLessThan15Percent_ThenCreatureLooksNearDeath()
		{
			var hitPoints = new HitPoints(100) { Current = 14 };
			var condition = HealthinessReader.Measure(hitPoints);

			Assert.AreEqual("Near Death", condition);
		}

		[TestMethod]
		public void WhenCurrentHitPointsIsZeroOrLess_ThenCreatureLooksDead()
		{
			var hitPoints = new HitPoints(100) { Current = 0 };
			var condition = HealthinessReader.Measure(hitPoints);

			Assert.AreEqual("Dead", condition);
		}
	}
}