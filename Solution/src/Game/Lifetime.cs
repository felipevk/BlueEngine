using System;
using System.Collections.Generic;
using System.Text;
using BlueEngine;
using BlueEngine.ECS;

public class LifetimeComponentData : IComponentData
{
	public int Life
	{ get; set; }

	public string GetNameId()
	{
		return "Lifetime";
	}
}
public class LifetimeComponentSystem : BlueEngine.ECS.ComponentSystem
{
	public override string GetNameId()
	{
		return "Lifetime";
	}

	protected override void Start( String gameObjectId, IComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( IsAlive( lifetimeData ) )
		{
			Log.Message( "Entity " + gameObjectId + " is alive" );
		}
	}

	protected override void Update( String gameObjectId, IComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( IsAlive( lifetimeData ) )
		{
			lifetimeData.Life -= 1;
			if ( scene.GetGameObject( gameObjectId ).HasComponentData(new PositionComponentData().GetNameId()) )
			{
				Microsoft.Xna.Framework.Vector2 position = scene.GetGameObject( gameObjectId ).GetComponentData<PositionComponentData>().position;
				position.X += 10;
				position.Y += 10;
			
				scene.GetGameObject( gameObjectId ).GetComponentData<PositionComponentData>().position = position;
			}

			if ( !IsAlive( lifetimeData ) )
			{
				Log.Message( "Entity " + gameObjectId + " is dead" );
			}
		}
	}

	public bool IsAlive( LifetimeComponentData data )
	{
		return data.Life >= 0;
	}
}
