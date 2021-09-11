using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Blue.ECS
{
	public class Scene
	{
		public Color BackgroundColor
		{ get; set; } = Color.CornflowerBlue;

		private Dictionary<String, GameObject> m_gameObjects = new Dictionary<string, GameObject>();
		private Dictionary<String, ManagedSystem> m_managedSystems = new Dictionary<string, ManagedSystem>();
		private Dictionary<String, ComponentSystem> m_componentSystems = new Dictionary<string, ComponentSystem>();
		private Dictionary<String, String> m_dataSystemMap = new Dictionary<string, String>();

		public Scene()
		{
			// Register Core Components
			RegisterComponent<SpriteComponentSystem, SpriteComponentData>();
			RegisterComponent<TextComponentSystem, TextComponentData>();
			RegisterComponent<ParticleComponentSystem, ParticleComponentData>();
			RegisterComponent<BoxCollision2DComponentSystem, BoxCollision2DComponentData>();
			RegisterComponent<SoundComponentSystem, SoundComponentData>();

			RegisterManagedSystems();
			RegisterComponents();
			RegisterGameObjects();
		}

		public virtual void Start()
		{
			foreach ( KeyValuePair<String, ManagedSystem> entry in m_managedSystems )
			{
				ManagedSystem system = entry.Value;
				system.Start();
			}
			foreach ( KeyValuePair<String, ComponentSystem> entry in m_componentSystems )
			{
				ComponentSystem system = entry.Value;
				system.Start();
			}
		}

		public virtual void LoadContent()
		{
		}

		protected virtual void RegisterManagedSystems()
		{
		}

		protected virtual void RegisterComponents()
		{
		}

		protected void RegisterManagedSystem<T>()
			where T : ManagedSystem, new()
		{
			if ( !m_managedSystems.ContainsKey( typeof( T ).ToString() ) )
			{
				T system = new T();
				m_managedSystems.Add( typeof( T ).ToString(), system );
			}
			else
			{
				// TODO Assert
			}
		}

		public bool HasManagedSystem<T>()
			where T : ManagedSystem
		{
			return m_managedSystems.ContainsKey( typeof( T ).ToString() );
		}

		public T GetManagedSystem<T>()
			where T : ManagedSystem
		{
			if ( HasManagedSystem<T>() )
			{
				return (T)m_managedSystems[typeof( T ).ToString()];
			}
			else
			{
				// TODO assert
				return default( T );
			}
		}

		protected void RegisterComponent<T, U>()
			where T : ComponentSystem, new()
			where U : ComponentData
		{
			T system = new T();
			if ( !m_componentSystems.ContainsKey( typeof( T ).ToString() ) )
			{
				m_componentSystems.Add( typeof( T ).ToString(), system );
				m_dataSystemMap.Add( typeof( U ).ToString(), typeof( T ).ToString() );
			}
			else
			{
				// TODO Assert
			}
		}

		protected virtual void RegisterGameObjects()
		{
		}

		public GameObject CreateGameObject( String name )
		{
			GameObject newGameObject = new GameObject( name );
			m_gameObjects.Add( newGameObject.Id, newGameObject );

			return newGameObject;
		}

		public GameObject GetGameObject( String uuid )
		{
			if ( m_gameObjects.ContainsKey( uuid ) )
			{
				return m_gameObjects[uuid];
			}
			else
			{
				// TODO Assert
				return null;
			}
		}

		public T CreateComponentData<T>( String gameObjectId )
			where T : ComponentData, new()
		{
			if ( HasComponentData<T>( gameObjectId ) )
				return GetComponentData<T>( gameObjectId );

			Attribute[] attrs = Attribute.GetCustomAttributes( typeof( T ) );
			foreach ( Attribute attr in attrs )
			{
				if ( attr is RequiresComponentData )
				{
					RequiresComponentData reqComp = (RequiresComponentData)attr;
					String reqSystemName = m_dataSystemMap[reqComp.required.FullName.ToString()];
					if ( !m_componentSystems[reqSystemName].Data.ContainsKey( gameObjectId ) )
					{
						m_componentSystems[reqSystemName].Data.Add( gameObjectId, (ComponentData)Activator.CreateInstance( reqComp.required ) );
					}
				}
			}

			T newComponentData = new T();

			String systemName = m_dataSystemMap[typeof( T ).ToString()];
			m_componentSystems[systemName].Data.Add( gameObjectId, newComponentData );

			return newComponentData;
		}

		public bool HasComponentData<T>( String gameObjectId )
			where T : ComponentData
		{
			String systemName = m_dataSystemMap[typeof( T ).ToString()];
			return m_componentSystems[systemName].Data.ContainsKey( gameObjectId );
		}

		public T GetComponentData<T>( String gameObjectId )
			where T : ComponentData
		{
			String systemName = m_dataSystemMap[typeof( T ).ToString()];
			if ( HasComponentData<T>( gameObjectId ) )
			{
				return (T)m_componentSystems[systemName].Data[gameObjectId];
			}
			else
			{
				// TODO assert
				return default( T );
			}
		}

		public void RemoveComponentData<T>( String gameObjectId )
			where T : ComponentData
		{
			String componentName = m_dataSystemMap[typeof( T ).ToString()];
			m_componentSystems[componentName].Data.Remove( gameObjectId );
		}

		public void Update()
		{
			foreach ( KeyValuePair<String, ManagedSystem> entry in m_managedSystems )
			{
				ManagedSystem system = entry.Value;
				system.Update();
			}
			foreach ( KeyValuePair<String, ComponentSystem> entry in m_componentSystems )
			{
				ComponentSystem system = entry.Value;
				system.Update();
			}
		}

		public void Render()
		{
			foreach ( KeyValuePair<String, ManagedSystem> entry in m_managedSystems )
			{
				ManagedSystem system = entry.Value;
				system.Render();
			}
			foreach ( KeyValuePair<String, ComponentSystem> entry in m_componentSystems )
			{
				ComponentSystem system = entry.Value;
				system.Render();
			}
		}

		public void ProcessCollisions( Collision2DManager.CollisionGlobalState collisionGlobalState )
		{
			foreach ( KeyValuePair<String, ComponentSystem> entry in m_componentSystems )
			{
				ComponentSystem system = entry.Value;
				system.ProcessCollisions( collisionGlobalState );
			}
		}
	}
}
