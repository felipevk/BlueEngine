using System;
using System.Collections.Generic;
using System.Text;

namespace BlueEngine.ECS
{
	public abstract class ComponentSystem
	{
		public ComponentSystem( String name ) => Name = name;

		public String Name
		{ get; set; } = "";

		public Dictionary<String, ComponentData> Data
		{ get; set; } = new Dictionary<string, ComponentData>();

		public void ForEachData( Action<String, ComponentData> action)
		{
			List<String> objectsToRemove = new List<string>();
			foreach ( KeyValuePair< String, ComponentData > entry in Data )
			{
				ComponentData gameObjectData = entry.Value;
				action( entry.Key, entry.Value);
			}
		}

		public void Start()
		{
			Action<String, ComponentData> startAction = ( gameObjectId, data ) => Start( gameObjectId, data );
			ForEachData( startAction );
		}
		protected virtual void Start( String gameObjectId, ComponentData data ) { }

		public void Clean()
		{
			Action<String, ComponentData> cleanAction = ( gameObjectId, data ) => Clean( gameObjectId, data );
			ForEachData( cleanAction );
		}
		protected virtual void Clean( String gameObjectId, ComponentData data ) { }

		public void Update()
		{
			Action<String, ComponentData> updateAction = ( gameObjectId, data ) => Update( gameObjectId, data );
			ForEachData( updateAction );
		}
		protected virtual void Update( String gameObjectId, ComponentData data ) { }

		public void Render()
		{
			Action<String, ComponentData> rencerAction = ( gameObjectId, data ) => Render( gameObjectId, data );
			ForEachData( rencerAction );
		}
		protected virtual void Render( String gameObjectId, ComponentData data ) { }

		public abstract String GetNameId();
	}
}
