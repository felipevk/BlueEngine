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

		protected override void LoadContent()
		{
			base.LoadContent();
		}
	}
	public class MainScene : BlueEngine.ECS.Scene
	{
		protected override void RegisterComponents()
		{
			base.RegisterComponents();

			RegisterComponent<LifetimeComponentSystem, LifetimeComponentData>();
		}

		protected override void RegisterGameObjects()
		{
			base.RegisterGameObjects();

			BlueEngine.ECS.GameObject player = RegisterGameObject( "Player" );
			BlueEngine.ECS.GameObject ball1 = RegisterGameObject( "Ball1" );
			BlueEngine.ECS.GameObject ball2 = RegisterGameObject( "Ball2" );

			CreateComponentData<LifetimeComponentData>( player.Id ).Life = 3000;
			CreateComponentData<LifetimeComponentData>( ball1.Id ).Life = 40;
			CreateComponentData<LifetimeComponentData>( ball2.Id ).Life = 5;

			CreateComponentData<BlueEngine.ECS.PositionComponentData>( ball1.Id ).position = new Microsoft.Xna.Framework.Vector2( 0, 0 );
			BlueEngine.ECS.SpriteComponentData spriteBall1 = CreateComponentData<BlueEngine.ECS.SpriteComponentData>( ball1.Id );
			spriteBall1.name = "ball";

			CreateComponentData<BlueEngine.ECS.PositionComponentData>( ball2.Id ).position = new Microsoft.Xna.Framework.Vector2( 200, 100 );
			BlueEngine.ECS.SpriteComponentData spriteBall2 = CreateComponentData<BlueEngine.ECS.SpriteComponentData>( ball2.Id );
			spriteBall2.name = "ball";
			spriteBall2.color = Microsoft.Xna.Framework.Color.Yellow;
		}
	};
}
