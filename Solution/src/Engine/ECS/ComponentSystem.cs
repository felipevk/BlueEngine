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

		public virtual void LoadContent() { }

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
			Action<String, ComponentData> renderAction = ( gameObjectId, data ) => Render( gameObjectId, data );
			ForEachData( renderAction );
		}
		protected virtual void Render( String gameObjectId, ComponentData data ) { }

		public void ProcessCollisions( Collision2DManager.CollisionGlobalState collisionState )
		{
			Action<String, ComponentData> triggerCollision = ( gameObjectId, data ) =>
			{
				if ( collisionState.collisionsPerGameObject.ContainsKey( gameObjectId ) )
				{
					Collision2DManager.GameObjectCollisions gameObjectCollisions = collisionState.collisionsPerGameObject[gameObjectId];
					foreach ( var colliderKvp in gameObjectCollisions.collisions )
					{
						switch ( colliderKvp.Value.state )
						{
							case Collision2DManager.CollisionState.Enter:
								OnCollision2DEnter( gameObjectId, data, colliderKvp.Key );
								break;
							case Collision2DManager.CollisionState.Stay:
								OnCollision2DStay( gameObjectId, data, colliderKvp.Key );
								break;
							case Collision2DManager.CollisionState.Exit:
								OnCollision2DExit( gameObjectId, data, colliderKvp.Key );
								break;
							default:
								break;
						}
					}
				}
			};
			ForEachData( triggerCollision );
		}

		protected virtual void OnCollision2DEnter( String gameObjectId, ComponentData data, String colliderId ) { }
		protected virtual void OnCollision2DStay( String gameObjectId, ComponentData data, String colliderId ) { }
		protected virtual void OnCollision2DExit( String gameObjectId, ComponentData data, String colliderId ) { }
	}
}
