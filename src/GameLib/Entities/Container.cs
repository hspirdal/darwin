using System.Collections.Generic;
using GameLib.Utility;

namespace GameLib.Entities
{
	public class Container : Item, IDeepCopy<Container>
	{
		public List<Item> Contents { get; set; }

		public Container()
			: base(id: string.Empty, name: string.Empty, subType: SubType.Container)
		{
		}

		public Container(string id, string name, string type)
			: base(id, name, SubType.Container)
		{
		}

		public new Container DeepCopy()
		{
			return new Container(string.Copy(Id), string.Copy(Name), string.Copy(Type));
		}
	}
}