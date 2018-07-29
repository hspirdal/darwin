namespace GameLib.Messaging
{
	public enum MessageTopic
	{
		Nothing,
		AttackedBy,
		Attacking,
		KilledBy,
		SuccessfulHitBy,
		FailedHitBy,
		CombatantFlees,
		CombatantDissapears,
		CombatantDies,
		ExperienceGain,
		MovementFailed,
	}
}