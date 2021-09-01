using System;

using Blue;
using Blue.ECS;
using Microsoft.Xna.Framework;

public class LifetimeComponentData : ComponentData
{
	public int Life
	{ get; set; }
}
public class LifetimeComponentSystem : ComponentSystem
{
	protected override void Start( String gameObjectId, ComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( IsAlive( lifetimeData ) )
		{
			GameObject gameObj = scene.GetGameObject( gameObjectId );
			Log.Message( "Entity " + gameObj.Name + " is alive" );
		}
	}

	protected override void Update( String gameObjectId, ComponentData data )
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
				if ( Scene.HasComponentData<SpriteComponentData>( gameObjectId ) )
				{
					Scene.GetComponentData<SpriteComponentData>( gameObjectId ).drawDebugColor = Color.Yellow;
				}
			}
		}
	}

	public bool IsAlive( LifetimeComponentData data )
	{
		return data.Life >= 0;
	}
}
