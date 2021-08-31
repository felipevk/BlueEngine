using Microsoft.Xna.Framework;

namespace Blue.ECS
{
	class PositionComponentData : IComponentData
	{
		public Vector2 position;
	}

	class PositionComponentSystem : ComponentSystem
	{
	}
}
