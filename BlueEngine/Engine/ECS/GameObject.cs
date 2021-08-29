using System;
using System.Collections.Generic;
using System.Text;

namespace BlueEngine.ECS
{
	public class GameObject
	{
		public GameObject( String id, String name )
		{
			Id = id;
			name = Name;
		}
		public static String GenerateGameObjectId()
		{
			return System.Guid.NewGuid().ToString();
		}

		public String Id
		{ get; set; }

		public String Name
		{ get; set; }

		public List<String> Children = new List<string>();
		public Dictionary<String, ComponentData> ComponentDataRegistry = new Dictionary<string, ComponentData>();

		public bool HasComponentData( String componentName )
		{
			return ComponentDataRegistry.ContainsKey( componentName );
		}

		public T GetComponent<T>( String componentName ) where T : ComponentData
		{
			ComponentData retrievedComponentData;
			if ( ComponentDataRegistry.TryGetValue( componentName, out retrievedComponentData ) )
			{
				return retrievedComponentData as T;
			}
			else
			{
				// TODO assert
				return null;
			}
		}

		public T AddComponent<T>( String componentName ) where T : ComponentData, new()
		{
			if ( !ComponentDataRegistry.ContainsKey( componentName ) )
			{
				T newComponentData = new T();
				ComponentDataRegistry.Add( componentName, newComponentData );
				Scene.RegisterGameObjectToComponent( componentName, Id, newComponentData );
				return newComponentData;
			}
			else
			{
				// TODO assert
				return null;
			}
		}

		public void RemoveComponent<T>( String componentName ) where T : ComponentData
		{
			ComponentData retrievedComponentData;
			if ( ComponentDataRegistry.ContainsKey( componentName ) )
			{
				ComponentDataRegistry.Remove( componentName );
				Scene.RemoveGameObjectFromComponent( componentName, Id );
			}
			else
			{
				// TODO assert
			}
		}
	}
}
