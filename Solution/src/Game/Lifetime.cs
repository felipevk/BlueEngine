using System;
using System.Collections.Generic;
using System.Text;
using BlueEngine;
using BlueEngine.ECS;

public class LifetimeComponentData : IComponentData
{
	public int Life
	{ get; set; }
}
public class LifetimeComponentSystem : BlueEngine.ECS.ComponentSystem
{
	protected override void Start( String gameObjectId, IComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( IsAlive( lifetimeData ) )
		{
			GameObject gameObj = scene.GetGameObject( gameObjectId );
			Log.Message( "Entity " + gameObj.Name + " is alive" );
		}
	}

	protected override void Update( String gameObjectId, IComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( IsAlive( lifetimeData ) )
		{
			lifetimeData.Life -= 1;
			GameObject gameObj = scene.GetGameObject( gameObjectId );
			if ( Scene.HasComponentData<PositionComponentData>( gameObjectId ) )
			{
				Microsoft.Xna.Framework.Vector2 position = Scene.GetComponentData<PositionComponentData>( gameObjectId ).position;
				position.X += 10;
				position.Y += 10;

				Scene.GetComponentData<PositionComponentData>( gameObjectId ).position = position;
			}

			if ( !IsAlive( lifetimeData ) )
			{
				Log.Message( "Entity " + gameObj.Name + " is dead" );
			}
		}
	}

	public bool IsAlive( LifetimeComponentData data )
	{
		return data.Life >= 0;
	}
}
