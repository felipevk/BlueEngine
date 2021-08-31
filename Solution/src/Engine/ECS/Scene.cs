using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.ECS
{
	public class Scene
	{
		private Dictionary<String, GameObject> m_gameObjects = new Dictionary<string, GameObject>();
		private static Dictionary<String, ComponentSystem> m_systems = new Dictionary<string, ComponentSystem>();
		private static Dictionary<String, String> m_dataSystemMap = new Dictionary<string, String>();

		public Scene()
		{
			// Register Core Components
			RegisterComponent<PositionComponentSystem, PositionComponentData>();
			RegisterComponent<SpriteComponentSystem, SpriteComponentData>();

			RegisterComponents();
			RegisterGameObjects();
		}

		public void Start()
		{
			foreach ( KeyValuePair<String, ComponentSystem> entry in m_systems )
			{
				ComponentSystem system = entry.Value;
				system.Start();
			}
		}

		public virtual void LoadContent()
		{
			SpriteComponentSystem spriteSystem = m_systems[typeof( SpriteComponentSystem ).ToString()] as SpriteComponentSystem;
			spriteSystem.LoadTextures();
		}

		protected virtual void RegisterComponents()
		{
		}

		protected void RegisterComponent<T, U>()
			where T : ComponentSystem, new()
			where U : IComponentData
		{
			T system = new T();
			system.scene = this;
			if ( !m_systems.ContainsKey( typeof( T ).ToString() ) )
			{
				m_systems.Add( typeof( T ).ToString(), system );
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

		protected GameObject RegisterGameObject( String name )
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

		public static T CreateComponentData<T>( String gameObjectId )
			where T : IComponentData, new()
		{
			T newComponentData = new T();

			String systemName = m_dataSystemMap[typeof( T ).ToString()];
			m_systems[systemName].Data.Add( gameObjectId, newComponentData );

			return newComponentData;
		}

		public static bool HasComponentData<T>( String gameObjectId )
			where T : IComponentData
		{
			String systemName = m_dataSystemMap[typeof( T ).ToString()];
			return m_systems[systemName].Data.ContainsKey( gameObjectId );
		}

		public static T GetComponentData<T>( String gameObjectId )
			where T : IComponentData
		{
			String systemName = m_dataSystemMap[typeof( T ).ToString()];
			if ( HasComponentData<T>( gameObjectId ) )
			{
				return (T)m_systems[systemName].Data[gameObjectId];
			}
			else
			{
				// TODO assert
				return default( T );
			}
		}

		public static void RemoveComponentData<T>( String gameObjectId )
			where T : IComponentData
		{
			String componentName = m_dataSystemMap[typeof( T ).ToString()];
			m_systems[componentName].Data.Remove( gameObjectId );
		}

		public void Update()
		{
			foreach ( KeyValuePair<String, ComponentSystem> entry in m_systems )
			{
				ComponentSystem system = entry.Value;
				system.Update();
			}
		}

		public void Render()
		{
			foreach ( KeyValuePair<String, ComponentSystem> entry in m_systems )
			{
				ComponentSystem system = entry.Value;
				system.Render();
			}
		}
	}
}
