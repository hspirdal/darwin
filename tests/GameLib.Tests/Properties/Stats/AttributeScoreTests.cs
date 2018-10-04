using GameLib.Properties.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLib.Tests.Properties.Stats
{
	[TestClass]
	public class AttributeScoreTests
	{
		[TestMethod]
		public void Modifier_WhenAttributeScoreIs12_ThenModifierIsOne()
		{
			var stat = new AttributeScore(12);

			var modifier = stat.Modifier;

			Assert.AreEqual(1, modifier);
		}

		[TestMethod]
		public void Modifier_WhenAttributeScoreIs8_ThenModifierIsMinusOne()
		{
			var stat = new AttributeScore(8);

			var modifier = stat.Modifier;

			Assert.AreEqual(-1, modifier);
		}

		[TestMethod]
		public void Modifier_WhenAttributeScoreIs18_ThenModifierIsFour()
		{
			var stat = new AttributeScore(12);

			var modifier = stat.Modifier;

			Assert.AreEqual(1, modifier);
		}

		[TestMethod]
		public void Modifier_WhenAttributeScoreIs2_ThenModifierIsMinusFour()
		{
			var stat = new AttributeScore(8);

			var modifier = stat.Modifier;

			Assert.AreEqual(-1, modifier);
		}
	}
}