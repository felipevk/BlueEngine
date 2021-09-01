﻿using System;
using System.Collections.Generic;

namespace Blue.ECS
{
	public abstract class ComponentSystem : ManagedSystem
	{
		public Scene scene
		{ get; set; }

		public Dictionary<String, IComponentData> Data
		{ get; set; } = new Dictionary<string, IComponentData>();

		public void ForEachData( Action<String, IComponentData> action )
		{
			foreach ( KeyValuePair<String, IComponentData> entry in Data )
			{
				IComponentData gameObjectData = entry.Value;
				action( entry.Key, entry.Value );
			}
		}

		public override void Start()
		{
			Action<String, IComponentData> startAction = ( gameObjectId, data ) => Start( gameObjectId, data );
			ForEachData( startAction );
		}
		protected virtual void Start( String gameObjectId, IComponentData data ) { }

		public override void Clean()
		{
			Action<String, IComponentData> cleanAction = ( gameObjectId, data ) => Clean( gameObjectId, data );
			ForEachData( cleanAction );
		}
		protected virtual void Clean( String gameObjectId, IComponentData data ) { }

		public override void Update()
		{
			Action<String, IComponentData> updateAction = ( gameObjectId, data ) => Update( gameObjectId, data );
			ForEachData( updateAction );
		}
		protected virtual void Update( String gameObjectId, IComponentData data ) { }

		public override void Render()
		{
			Action<String, IComponentData> rencerAction = ( gameObjectId, data ) => Render( gameObjectId, data );
			ForEachData( rencerAction );
		}
		protected virtual void Render( String gameObjectId, IComponentData data ) { }
	}
}
