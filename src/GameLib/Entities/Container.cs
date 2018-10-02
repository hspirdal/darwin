using System.Collections.Generic;
using System.Linq;
using GameLib.Utility;

namespace GameLib.Entities
{
	public class Container : Item, IDeepCopy<Container>
	{
		public List<Item> Contents { get; set; }

		public Container()
			: this(id: string.Empty, name: string.Empty, contents: new List<Item>())
		{
		}

		public Container(string id, string name)
			: this(id, name, new List<Item>())
		{
		}

		public Container(string id, string name, List<Item> contents)
			: base(id, name, SubType.Container)
		{
		}

		public new Container DeepCopy()
		{
			var contents = Contents.Select(i => i.DeepCopy()).ToList();
			return new Container(string.Copy(Id), string.Copy(Name), contents);
		}
	}
}