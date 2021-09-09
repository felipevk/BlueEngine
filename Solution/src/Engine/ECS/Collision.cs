using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Blue.ECS
{
	public struct AABB2D
	{
		public String gameObjectId;
		public Vector2 min;
		public Vector2 max;

		public AABB2D( String gameObjectIdIn )
		{
			gameObjectId = gameObjectIdIn;
			min = Vector2.Zero;
			max = Vector2.Zero;
		}
	}
	public class BoxCollision2DComponentData : ComponentData
	{
		public int Width = 0;
		public int Height = 0;
		public bool drawDebug = false;
		public int collisions = 0;
	}

	public class BoxCollision2DComponentSystem : ComponentSystem
	{
		protected override void Update( String gameObjectId, ComponentData data )
		{
			BoxCollision2DComponentData boxCollision2DData = data as BoxCollision2DComponentData;
			Collision2DManager.AddAABB2D( GetAABB( gameObjectId, boxCollision2DData ) );
		}

		protected override void Render( String gameObjectId, ComponentData data )
		{
			BoxCollision2DComponentData boxCollision2DData = data as BoxCollision2DComponentData;
			if ( boxCollision2DData.drawDebug )
			{
				Rectangle globalRectangle = new Rectangle( 0, 0, boxCollision2DData.Width, boxCollision2DData.Height );
				globalRectangle.X = (int)scene.GetGameObject( gameObjectId ).GetGlobalPosition().X - boxCollision2DData.Width / 2;
				globalRectangle.Y = (int)scene.GetGameObject( gameObjectId ).GetGlobalPosition().Y - boxCollision2DData.Height / 2;
				Color debugColor = boxCollision2DData.collisions > 0 ? Color.Red : Color.Green;
				debugColor.A = 128;
				Game.Instance.GameRenderer.PrepareToDrawRectangle(
					globalRectangle,
					debugColor,
					true);
			}
		}

		public AABB2D GetAABB( String gameObjectId, BoxCollision2DComponentData data )
		{
			Vector3 globalPosition = scene.GetGameObject( gameObjectId ).GetGlobalPosition();

			AABB2D aabb = new AABB2D( gameObjectId );
			aabb.min.X = globalPosition.X - data.Width / 2;
			aabb.min.Y = globalPosition.Y - data.Height / 2;

			aabb.max.X = globalPosition.X + data.Width / 2;
			aabb.max.Y = globalPosition.Y + data.Height / 2;

			return aabb;
		}

		protected override void OnCollision2DEnter( String gameObjectId, ComponentData data, String colliderId )
		{
			BoxCollision2DComponentData boxCollision2DData = data as BoxCollision2DComponentData;
			boxCollision2DData.collisions += 1;
		}

		protected override void OnCollision2DExit( String gameObjectId, ComponentData data, String colliderId )
		{
			BoxCollision2DComponentData boxCollision2DData = data as BoxCollision2DComponentData;
			boxCollision2DData.collisions -= 1;
		}
	}

	public static class Collision2DManager
	{
		public enum CollisionState
		{
			Enter,
			Stay,
			Exit
		}
		public enum BoundType
		{
			Min,
			Max
		}

		public class CollisionGlobalState
		{
			public Dictionary<String, GameObjectCollisions> collisionsPerGameObject = new Dictionary<String, GameObjectCollisions>();

			public void DeleteExpiredCollisions()
			{
				foreach ( var kvp in collisionsPerGameObject )
				{
					kvp.Value.DeleteExpiredCollisions();
				}
			}

			public void UnvisitCollisions()
			{
				foreach ( var kvp in collisionsPerGameObject )
				{
					kvp.Value.UnvisitCollisions();
				}
			}

			public void UpdateUnvisitedCollisions()
			{
				foreach ( var kvp in collisionsPerGameObject )
				{
					kvp.Value.UpdateUnvisitedCollisions();
				}
			}

			public void AddNewCollisions( Dictionary<String, List<String>> newCollisions )
			{
				foreach ( var newCollisionsGameObject in newCollisions )
				{
					String gameObjectId = newCollisionsGameObject.Key;

					if ( collisionsPerGameObject.ContainsKey( gameObjectId ) )
					{
						GameObjectCollisions gameObjectCollisions = collisionsPerGameObject[gameObjectId];
						foreach ( String collider in newCollisionsGameObject.Value )
						{
							gameObjectCollisions.AddOrUpdateCollision( collider );
						}
					}
					else
					{
						// all collisions in this gameobject are new
						GameObjectCollisions gameObjectCollisions = new GameObjectCollisions();
						foreach ( String collider in newCollisionsGameObject.Value )
						{
							GameObjectCollisionState newState = new GameObjectCollisionState();
							newState.visited = true;
							gameObjectCollisions.collisions.Add( collider, newState );
						}

						collisionsPerGameObject.Add( gameObjectId, gameObjectCollisions );
					}
				}
			}
		}

		public class GameObjectCollisions
		{
			public Dictionary<String, GameObjectCollisionState> collisions = new Dictionary<String, GameObjectCollisionState>();

			public void DeleteExpiredCollisions()
			{
				List<String> collisionsToDelete = new List<string>();
				foreach ( var kvp in collisions )
				{
					if ( kvp.Value.state == CollisionState.Exit )
					{
						collisionsToDelete.Add( kvp.Key );
					}
				}

				foreach ( String collisionToDelete in collisionsToDelete )
				{
					collisions.Remove( collisionToDelete );
				}
			}

			public void UnvisitCollisions()
			{
				foreach ( var kvp in collisions )
				{
					kvp.Value.visited = false;
				}
			}

			public void AddOrUpdateCollision( String newCollider )
			{
				if ( collisions.ContainsKey( newCollider ) )
				{
					collisions[newCollider].visited = true;
					collisions[newCollider].state = CollisionState.Stay;
				}
				else
				{
					GameObjectCollisionState newState = new GameObjectCollisionState();
					newState.visited = true;
					collisions.Add( newCollider, newState );
				}
			}

			public void UpdateUnvisitedCollisions()
			{
				foreach ( var kvp in collisions )
				{
					if ( !kvp.Value.visited )
					{
						kvp.Value.visited = true;
						kvp.Value.state = CollisionState.Exit;
					}
				}
			}
		}

		public class GameObjectCollisionState
		{
			public CollisionState state = CollisionState.Enter;
			public bool visited = false;
		}

		private static CollisionGlobalState _globalState = new CollisionGlobalState();

		static List<AABB2D> _aabb2d = new List<AABB2D>();

		public struct AABB2DBound
		{
			public String gameObjectId;
			public float bound;
			public BoundType boundType;

			public AABB2DBound( String gameObjectIdIn, float boundIn, BoundType boundTypeIn )
			{
				gameObjectId = gameObjectIdIn;
				bound = boundIn;
				boundType = boundTypeIn;
			}
		}

		public static CollisionGlobalState GetGlobalState()
		{
			return _globalState;
		}

		public static void AddAABB2D( AABB2D aabb )
		{
			_aabb2d.Add( aabb );
		}

		public static void Update()
		{
			_globalState.DeleteExpiredCollisions();
			_globalState.UnvisitCollisions();

			List<Tuple<String, String>> collisionRequests = GetCollisionRequests();

			Dictionary < String, List < String >> currentCollisions = GetCurrentCollisions( collisionRequests );

			if ( currentCollisions.Count > 0 )
			{
				_globalState.AddNewCollisions( currentCollisions );
			}

			_globalState.UpdateUnvisitedCollisions();

			_aabb2d.Clear();
		}

		private static List<Tuple<String, String>> GetCollisionRequests()
		{
			List<AABB2DBound> bounds = new List<AABB2DBound>();
			foreach ( var aabb in _aabb2d )
			{
				bounds.Add( new AABB2DBound( aabb.gameObjectId, aabb.min.X, BoundType.Min ) );
				bounds.Add( new AABB2DBound( aabb.gameObjectId, aabb.max.X, BoundType.Max ) );
			}
			List<AABB2DBound> sortedBounds = bounds.OrderBy( o => o.bound ).ToList();

			List<String> activeAABBs = new List<string>();
			List<Tuple<String, String>> collisionRequests = new List<Tuple<String, String>>();

			foreach ( var aabbBounds in sortedBounds )
			{
				switch ( aabbBounds.boundType )
				{
					case BoundType.Min:
						{
							activeAABBs.Add( aabbBounds.gameObjectId );
							break;
						}
					case BoundType.Max:
						{
							activeAABBs.Remove( aabbBounds.gameObjectId );
							foreach ( var activeAABBGameObjectId in activeAABBs )
							{
								collisionRequests.Add( new Tuple<string, string>( aabbBounds.gameObjectId, activeAABBGameObjectId ) );
							}
							break;
						}
					default:
						break;
				}
			}

			if ( activeAABBs.Count > 0 )
			{
				// TODO assert
			}

			return collisionRequests;
		}

		private static Dictionary<String, List<String>> GetCurrentCollisions( List<Tuple<String, String>> collisionRequests )
		{
			Dictionary<String, List<String>> collisions = new Dictionary<string, List<string>>();

			foreach ( var collisionRequest in collisionRequests )
			{
				// TODO implement proper individual collision with correct scene reference
				BoxCollision2DComponentData boxCol1 = Scene.GetComponentData<BoxCollision2DComponentData>( collisionRequest.Item1 );
				Rectangle rect1 = new Rectangle( 0, 0, boxCol1.Width, boxCol1.Height );
				rect1.X = (int)Game.Instance.CurrentScene.GetGameObject( collisionRequest.Item1 ).GetGlobalPosition().X - rect1.Width / 2;
				rect1.Y = (int)Game.Instance.CurrentScene.GetGameObject( collisionRequest.Item1 ).GetGlobalPosition().Y - rect1.Height / 2;

				BoxCollision2DComponentData boxCol2 = Scene.GetComponentData<BoxCollision2DComponentData>( collisionRequest.Item2 );
				Rectangle rect2 = new Rectangle( 0, 0, boxCol2.Width, boxCol2.Height );
				rect2.X = (int)Game.Instance.CurrentScene.GetGameObject( collisionRequest.Item2 ).GetGlobalPosition().X - rect2.Width / 2;
				rect2.Y = (int)Game.Instance.CurrentScene.GetGameObject( collisionRequest.Item2 ).GetGlobalPosition().Y - rect2.Height / 2;

				bool collides = rect1.Intersects( rect2 );

				if ( collides )
				{
					List<String> colliders;
					if ( !collisions.TryGetValue( collisionRequest.Item1, out colliders ) )
					{
						colliders = new List<string>();
					}
					colliders.Add( collisionRequest.Item2 );
					collisions[collisionRequest.Item1] = colliders;
					
					if ( !collisions.TryGetValue( collisionRequest.Item2, out colliders ) )
					{
						colliders = new List<string>();
					}
					colliders.Add( collisionRequest.Item1 );
					collisions[collisionRequest.Item2] = colliders;
				}
			}

			return collisions;
		}
	}
}
