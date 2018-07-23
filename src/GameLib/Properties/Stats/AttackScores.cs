using GameLib.Entities;
using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class AttackScores : IDeepCopy<AttackScores>
	{
		Weapon Primary { get; set; }
		AttributeScore BaseAttackBonus { get; set; }

		public AttackScores()
		{
			BaseAttackBonus = new AttributeScore(1);
		}

		public AttackScores(Weapon weapon, AttributeScore baseAttackBonus)
		{
			Primary = weapon;
			BaseAttackBonus = baseAttackBonus;
		}

		public AttackScores(Weapon weapon, int baseAttackBonus)
		{
			Primary = weapon;
			BaseAttackBonus = new AttributeScore(baseAttackBonus);
		}

		public AttackScores DeepCopy()
		{
			return new AttackScores(Primary?.DeepCopy(), BaseAttackBonus.DeepCopy());
		}
	}
}