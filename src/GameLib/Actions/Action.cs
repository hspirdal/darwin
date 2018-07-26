namespace GameLib.Actions
{
  public class Action
  {
    public string OwnerId { get; set; }
    public string Name { get; set; }
    public int CooldownDurationMilisecs { get; set; }

    public Action() { }

    public Action(string ownerId, string name, int cooldownDurationMilisecs)
    {
      OwnerId = ownerId;
      Name = name;
      CooldownDurationMilisecs = cooldownDurationMilisecs;
    }

    public override string ToString()
    {
      return $"{nameof(OwnerId)}: '{OwnerId}, {nameof(Name)}: '{Name}', {nameof(CooldownDurationMilisecs)}: '{CooldownDurationMilisecs}'";
    }
  }
}