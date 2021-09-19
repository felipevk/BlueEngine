using System;

namespace Blue.ECS
{
	public abstract class ManagedSystem
	{
		public virtual void Start() { }

		public virtual void Clean() { }

		public virtual void Update() { }

		public virtual void Render() { }

		protected GameObject CreateGameObject( String name )
		{
			return Game.Instance.CurrentScene.CreateGameObject( name );
		}

		protected void DestroyGameObject( String name )
		{
			Game.Instance.CurrentScene.DestroyGameObject( name );
		}

		public GameObject GetGameObject( String uuid )
		{
			return Game.Instance.CurrentScene.GetGameObject( uuid );
		}

		public T CreateComponentData<T>( String gameObjectId )
			where T : ComponentData, new()
		{
			return Game.Instance.CurrentScene.CreateComponentData<T>( gameObjectId );
		}

		public bool HasComponentData<T>( String gameObjectId )
			where T : ComponentData
		{
			return Game.Instance.CurrentScene.HasComponentData<T>( gameObjectId );
		}

		public T GetComponentData<T>( String gameObjectId )
			where T : ComponentData
		{
			return Game.Instance.CurrentScene.GetComponentData<T>( gameObjectId );
		}

		public void RemoveComponentData<T>( String gameObjectId )
			where T : ComponentData
		{
			Game.Instance.CurrentScene.RemoveComponentData<T>( gameObjectId );
		}
	}
}
