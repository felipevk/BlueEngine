using Microsoft.Xna.Framework;

namespace BlueEngine.ECS
{
	class PositionComponentData : IComponentData
	{
		public string GetNameId()
		{
			return "Position";
		}

		public Vector2 position;
	}

	class PositionComponentSystem : ComponentSystem
	{
		public override string GetNameId()
		{
			return "Position";
		}
	}
}
