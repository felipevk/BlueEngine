using System;
using System.Collections.Generic;
using System.Text;

namespace BlueEngine.ECS
{
	public interface IComponentData
	{
		/// <summary>
		/// Unique name identifier that needs to match NameId on correspondent ComponentSystem
		/// </summary>
		/// <returns></returns>
		String GetNameId();
	}
}
