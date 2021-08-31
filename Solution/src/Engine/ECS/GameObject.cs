﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlueEngine.ECS
{
	public class GameObject
	{
		public GameObject( String id, String name )
		{
			Id = id;
			Name = name;
		}
		public static String GenerateGameObjectId()
		{
			return System.Guid.NewGuid().ToString();
		}

		public String Id
		{ get; set; }

		public String Name
		{ get; set; }

		public List<String> Children = new List<string>();
	}
}
