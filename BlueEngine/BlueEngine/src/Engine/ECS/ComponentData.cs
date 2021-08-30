using System;
using System.Collections.Generic;
using System.Text;

namespace BlueEngine.ECS
{
	public abstract class ComponentData
	{
		/// <summary>
		/// Unique name identifier that needs to match NameId on correspondent ComponentSystem
		/// </summary>
		/// <returns></returns>
		public abstract String GetNameId();
	}
}
