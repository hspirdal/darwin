namespace GameLib.Combat
{
	public class AttackResult
	{
		public bool SuccessfulHit { get; set; }

		public int ToHitRoll { get; set; }
		public int ToHitModifier { get; set; }
		public int ToHitTotal => ToHitRoll + ToHitModifier;

		public int DamageRoll { get; set; }
		public int DamageModifier { get; set; }
		public int DamageTotal => DamageRoll + DamageModifier;

		public AttackResult(int toHitModifier, int damageModifier)
		{
			ToHitModifier = toHitModifier;
			DamageModifier = damageModifier;
		}
	}
}