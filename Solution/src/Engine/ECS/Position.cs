using Microsoft.Xna.Framework;

namespace BlueEngine.ECS
{
	class PositionComponentData : IComponentData
	{
		public Vector2 position;
	}

	class PositionComponentSystem : ComponentSystem
	{
	}
}
