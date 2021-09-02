using System;
using System.Collections.Generic;

using Blue.Core;
using Microsoft.Xna.Framework;

namespace Blue.ECS
{
	public class GameObject
	{
		public static String GenerateGameObjectId()
		{
			return System.Guid.NewGuid().ToString();
		}

		public GameObject( String id, String name )
		{
			Id = id;
			Name = name;
		}

		public GameObject( String name ) : this( GenerateGameObjectId(), name )
		{
		}

		public void AddChild( GameObject childObject )
		{
			Children.Add( childObject.Id );
			childObject.Parent = Id;
		}

		public Vector3 GetGlobalPosition()
		{
			if ( Parent == null )
				return Transform.Position;

			Vector3 parentGlobalPosition = Game.Instance.CurrentScene.GetGameObject( Parent ).GetGlobalPosition();

			return Transform.Position + parentGlobalPosition;
		}

		public Vector3 GetGlobalScale()
		{
			if ( Parent == null )
				return Transform.Scale;

			Vector3 Scale = Game.Instance.CurrentScene.GetGameObject( Parent ).GetGlobalScale();

			return Transform.Scale * Scale;
		}

		public String Id
		{ get; set; }

		public String Name
		{ get; set; }

		public Transform Transform
		{ get; set; } = new Transform();

		public String Parent
		{ get; set; } = null;

		public List<String> Children
		{ get; set; } = new List<string>();
	}
}
