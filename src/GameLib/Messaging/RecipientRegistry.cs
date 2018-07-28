using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib.Messaging
{
	public interface IRecipientRegistry
	{
		bool Contains(string id);
		IMessageRecipient Get(string id);
		void Register(IMessageRecipient receiver);
		void Remove(string id);
	}

	public class RecipientRegistry : AbstractRegistry<IMessageRecipient>, IRecipientRegistry
	{

	}
}