using System;

using BlueEngine;

class Program
{
	static void Main( string[] args )
	{
		Game game = new Game();
		game.CurrentScene = new MainScene();
		game.Run();
	}

	public class MainScene : BlueEngine.ECS.Scene
	{
		protected override void RegisterComponents()
		{
			base.RegisterComponents();

			RegisterComponent<LifetimeComponentSystem>();
		}

		protected override void RegisterGameObjects()
		{
			base.RegisterGameObjects();

			BlueEngine.ECS.GameObject player = RegisterGameObject( "Player" );
			BlueEngine.ECS.GameObject ball1 = RegisterGameObject( "Ball1" );
			BlueEngine.ECS.GameObject ball2 = RegisterGameObject( "Ball2" );

			player.AddComponentData<LifetimeComponentData>().Life = 3000;
			ball1.AddComponentData<LifetimeComponentData>().Life = 40;
			ball2.AddComponentData<LifetimeComponentData>().Life = 5;
		}
	};
}
