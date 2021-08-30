using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueEngine.ECS
{
	public class Scene
	{
		private Dictionary<String, GameObject> m_gameObjects = new Dictionary<string, GameObject>();
		private static Dictionary<String, ComponentSystem> m_systems = new Dictionary<string, ComponentSystem>();

		public void Start()
		{
			RegisterComponents();
			RegisterGameObjects();

			foreach ( KeyValuePair<String, ComponentSystem> entry in m_systems )
			{
				ComponentSystem system = entry.Value;
				system.Start();
			}
		}

		protected virtual void RegisterComponents()
		{
		}

		protected void RegisterComponent<T>() where T : ComponentSystem, new()
		{
			T component = new T();
			if ( !m_systems.ContainsKey( component.GetNameId() ) )
			{
				m_systems.Add( component.GetNameId(), component );
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
			String id = GameObject.GenerateGameObjectId();
			GameObject newGameObject = new GameObject( id, name );
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

		public static void RegisterGameObjectToComponent( String componentName, String gameObjectId, IComponentData componentData )
		{
			m_systems[componentName].Data.Add( gameObjectId, componentData );
		}

		public static void RemoveGameObjectFromComponent( String componentName, String gameObjectId )
		{
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
