using System;
using System.Collections.Generic;
using System.Text;
using BlueEngine;
using BlueEngine.ECS;

public class LifetimeComponentData : BlueEngine.ECS.ComponentData
{
	public int Life
	{ get; set; }

	public override string GetNameId()
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

	protected override void Update( String gameObjectId, ComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( lifetimeData.Life == 0 )
		{
			Log.Message( "Entity " + gameObjectId + " is dead" );
		}
		else
		{
			lifetimeData.Life -= 1;
			if ( lifetimeData.Life == 0 )
			{
				Log.Message( "Entity " + gameObjectId + " is dead" );
			}
			else
			{
				Log.Message( "Entity " + gameObjectId + " has " + lifetimeData.Life + " life left" );
			}
		}
	}
}
