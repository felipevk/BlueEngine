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
		public Dictionary<String, IComponentData> ComponentDataRegistry = new Dictionary<string, IComponentData>();

		public bool HasComponentData( String componentName )
		{
			return ComponentDataRegistry.ContainsKey( componentName );
		}

		public T GetComponentData<T>() where T : IComponentData, new()
		{
			// TODO find static way to get nameId
			String componentName = new T().GetNameId();
			IComponentData retrievedComponentData;
			if ( ComponentDataRegistry.TryGetValue( componentName, out retrievedComponentData ) )
			{
				return ( T )retrievedComponentData;
			}
			else
			{
				// TODO assert
				return default( T );
			}
		}

		public T AddComponentData<T>() where T : IComponentData, new()
		{
			T newComponentData = new T();
			if ( !ComponentDataRegistry.ContainsKey( newComponentData.GetNameId() ) )
			{
				ComponentDataRegistry.Add( newComponentData.GetNameId(), newComponentData );
				Scene.RegisterGameObjectToComponent( newComponentData.GetNameId(), Id, newComponentData );
				return newComponentData;
			}
			else
			{
				// TODO assert
				return default( T );
			}
		}

		public void RemoveComponentData<T>() where T : IComponentData, new()
		{
			// TODO find static way to get nameId
			String componentName = new T().GetNameId();
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
