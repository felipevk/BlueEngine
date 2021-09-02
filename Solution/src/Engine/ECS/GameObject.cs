using System;
using System.Collections.Generic;

using Blue.Core;

namespace Blue.ECS
{
	public class GameObject
	{
		public GameObject( String id, String name )
		{
			Id = id;
			Name = name;
		}

		public GameObject( String name ) : this( GenerateGameObjectId(), name )
		{
		}

		public static String GenerateGameObjectId()
		{
			return System.Guid.NewGuid().ToString();
		}

		public String Id
		{ get; set; }

		public String Name
		{ get; set; }

		public Transform Transform
		{ get; set; } = new Transform();

		public List<String> Children = new List<string>();
	}
}
