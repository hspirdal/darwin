using GameLib.Utility;

namespace GameLib.Entities
{
	public class Entity : IDeepCopy<Entity>
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }

		public Entity()
		{
			Id = string.Empty;
			Name = string.Empty;
			Type = string.Empty;
		}

		public Entity(string id, string name, string type)
		{
			Id = id;
			Name = name;
			Type = type;
		}

		public Entity DeepCopy()
		{
			return new Entity(string.Copy(Id), string.Copy(Name), string.Copy(Type));
		}
	}
}