using System.Linq;
using GameLib.Dice;
using GameLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Dice
{
	[TestClass]
	public class DiceRollerTests
	{
		private Mock<IRandomNumberGenerator> _rng;
		private DiceRoller _diceRoller;

		[TestInitialize]
		public void GivenADiceRoller()
		{
			_rng = new Mock<IRandomNumberGenerator>();
			_diceRoller = new DiceRoller(_rng.Object);
		}

		[TestMethod]
		public void WhenRollingMultipleD12_ThenResultReflectsCorrectTypeAndCorrectResult()
		{
			var fakeDiceResult = 12;
			var numberOfDice = 3;
			var expectedDiceType = DiceType.d12;

			_rng.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(fakeDiceResult);
			var result = _diceRoller.Roll(expectedDiceType, numberOfDice);

			var expectedTotal = fakeDiceResult * numberOfDice;

			Assert.AreEqual(3, result.Results.Count(i => i.DiceType == DiceType.d12));
			Assert.AreEqual(expectedTotal, result.Total);
		}

		[TestMethod]
		public void WhenRollingD20_ThenResultReflectsCorrectTypeAndCorrectResult()
		{
			var expectedResult = 20;
			var expectedDiceType = DiceType.d20;
			_rng.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);

			var result = _diceRoller.D20();

			Assert.AreEqual(expectedDiceType, result.DiceType);
			Assert.AreEqual(expectedResult, result.Result);
		}

		[TestMethod]
		public void WhenRollingD12_ThenResultReflectsCorrectTypeAndCorrectResult()
		{
			var expectedResult = 12;
			var expectedDiceType = DiceType.d12;
			_rng.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);

			var result = _diceRoller.D12();

			Assert.AreEqual(expectedDiceType, result.DiceType);
			Assert.AreEqual(expectedResult, result.Result);
		}

		[TestMethod]
		public void WhenRollingD10_ThenResultReflectsCorrectTypeAndCorrectResult()
		{
			var expectedResult = 10;
			var expectedDiceType = DiceType.d10;
			_rng.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);

			var result = _diceRoller.D10();

			Assert.AreEqual(expectedDiceType, result.DiceType);
			Assert.AreEqual(expectedResult, result.Result);
		}

		[TestMethod]
		public void WhenRollingD8_ThenResultReflectsCorrectTypeAndCorrectResult()
		{
			var expectedResult = 8;
			var expectedDiceType = DiceType.d8;
			_rng.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);

			var result = _diceRoller.D8();

			Assert.AreEqual(expectedDiceType, result.DiceType);
			Assert.AreEqual(expectedResult, result.Result);
		}

		[TestMethod]
		public void WhenRollingD6_ThenResultReflectsCorrectTypeAndCorrectResult()
		{
			var expectedResult = 6;
			var expectedDiceType = DiceType.d6;
			_rng.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);

			var result = _diceRoller.D6();

			Assert.AreEqual(expectedDiceType, result.DiceType);
			Assert.AreEqual(expectedResult, result.Result);
		}

		[TestMethod]
		public void WhenRollingD4_ThenResultReflectsCorrectTypeAndCorrectResult()
		{
			var expectedResult = 4;
			var expectedDiceType = DiceType.d4;
			_rng.Setup(i => i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedResult);

			var result = _diceRoller.D4();

			Assert.AreEqual(expectedDiceType, result.DiceType);
			Assert.AreEqual(expectedResult, result.Result);
		}
	}
}