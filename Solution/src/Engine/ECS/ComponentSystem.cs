using System;
using System.Collections.Generic;

namespace Blue.ECS
{
	public abstract class ComponentSystem : ManagedSystem
	{
		public Scene scene
		{ get; set; }

		public Dictionary<String, ComponentData> Data
		{ get; set; } = new Dictionary<string, ComponentData>();

		public void ForEachData( Action<String, ComponentData> action )
		{
			foreach ( KeyValuePair<String, ComponentData> entry in Data )
			{
				ComponentData gameObjectData = entry.Value;
				if ( gameObjectData.enabled )
				{
					action( entry.Key, entry.Value );
				}
			}
		}

		public override void Start()
		{
			Action<String, ComponentData> startAction = ( gameObjectId, data ) => Start( gameObjectId, data );
			ForEachData( startAction );
		}
		protected virtual void Start( String gameObjectId, ComponentData data ) { }

		public override void Clean()
		{
			Action<String, ComponentData> cleanAction = ( gameObjectId, data ) => Clean( gameObjectId, data );
			ForEachData( cleanAction );
		}
		protected virtual void Clean( String gameObjectId, ComponentData data ) { }

		public override void Update()
		{
			Action<String, ComponentData> updateAction = ( gameObjectId, data ) => Update( gameObjectId, data );
			ForEachData( updateAction );
		}
		protected virtual void Update( String gameObjectId, ComponentData data ) { }

		public override void Render()
		{
			Action<String, ComponentData> rencerAction = ( gameObjectId, data ) => Render( gameObjectId, data );
			ForEachData( rencerAction );
		}
		protected virtual void Render( String gameObjectId, ComponentData data ) { }
	}
}
