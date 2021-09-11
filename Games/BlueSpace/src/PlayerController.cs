using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public class PlayerControllerComponentData : ComponentData
	{
		public Vector2 direction
		{ get; set; } = Vector2.Zero;
	}

	public class PlayerControllerComponentSystem : ComponentSystem
	{
		public static float Speed = 500;
		protected override void Update( String gameObjectId, ComponentData data )
		{
			PlayerControllerComponentData playerControllerData = data as PlayerControllerComponentData;

			Vector2 direction = playerControllerData.direction;

			if ( Input.IsButtonDown( "right", 0 ) )
			{
				direction.X = 1;
			}
			else if ( Input.IsButtonDown( "left", 0 ) )
			{
				direction.X = -1;
			}
			else
			{
				direction.X = 0;
			}

			if ( Input.IsButtonDown( "up", 0 ) )
			{
				direction.Y = -1;
			}
			else if ( Input.IsButtonDown( "down", 0 ) )
			{
				direction.Y = 1;
			}
			else
			{
				direction.Y = 0;
			}

			playerControllerData.direction = direction;

			GameObject player = GetGameObject( gameObjectId );
			Vector3 playerPos = player.Transform.Position;
			playerPos.X += playerControllerData.direction.X * Speed * Time.DeltaTime;
			playerPos.Y += playerControllerData.direction.Y * Speed * Time.DeltaTime;

			player.Transform.Position = playerPos;
		}
	} 
}
