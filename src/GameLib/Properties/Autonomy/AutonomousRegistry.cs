using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib.Properties.Autonomy
{
	public interface IAutonomousRegistry
	{
		bool Contains(string id);
		IAutonomous Get(string id);
		List<IAutonomous> GetAll();
		void Register(IAutonomous autonomous);
		void Remove(string id);
	}

	public class AutonomousRegistry : AbstractRegistry<IAutonomous>, IAutonomousRegistry
	{

	}
}