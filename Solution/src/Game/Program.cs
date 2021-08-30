using System;

/// <summary>
/// The main class.
/// </summary>
public static class Program
{
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	static void Main()
	{
		var factory = new MonoGame.Framework.GameFrameworkViewSource<MyGame>();
		Windows.ApplicationModel.Core.CoreApplication.Run( factory );
	}

	public class MyGame : BlueEngine.Game
	{
		protected override void Initialize()
		{
			CurrentScene = new MainScene();
			base.Initialize();
		}
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
