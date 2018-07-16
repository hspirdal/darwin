namespace GameLib.Actions
{
	public class Action
	{
		public string OwnerId { get; set; }
		public string Name { get; set; }

		public virtual bool IsValid()
		{
			return !string.IsNullOrEmpty(OwnerId) && !string.IsNullOrEmpty(Name);
		}

		public override string ToString()
		{
			return $"{nameof(OwnerId)}: '{OwnerId}, {nameof(Name)}: '{Name}'";
		}
	}
}