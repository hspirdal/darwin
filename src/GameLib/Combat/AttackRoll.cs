using GameLib.Entities;

namespace GameLib.Combat
{
	public class AttackRoll
	{
		public Weapon Primary { get; set; }
		public int DamageModifier { get; set; }
		public int ToHitModifier { get; set; }

		public override string ToString()
		{
			return $"{Primary.Name} +{ToHitModifier} ({Primary.TimesApplied}{Primary.DiceType}+{DamageModifier})";
		}
	}
}