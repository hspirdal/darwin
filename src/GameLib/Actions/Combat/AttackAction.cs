namespace GameLib.Actions.Combat
{
	public class AttackAction : Action
	{
		public string TargetId { get; set; }

		public override bool IsValid()
		{
			return base.IsValid() && !string.IsNullOrEmpty(TargetId);
		}

		public override string ToString()
		{
			return base.ToString() + $", {nameof(TargetId)}: '{TargetId}'";
		}
	}
}