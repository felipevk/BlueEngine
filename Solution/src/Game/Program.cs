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

	public class MyGame : Blue.Game
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
	public class MainScene : Blue.ECS.Scene
	{
		protected override void RegisterComponents()
		{
			base.RegisterComponents();

			RegisterComponent<LifetimeComponentSystem, LifetimeComponentData>();
		}

		protected override void RegisterGameObjects()
		{
			base.RegisterGameObjects();

			Blue.ECS.GameObject player = RegisterGameObject( "Player" );
			Blue.ECS.GameObject ball1 = RegisterGameObject( "Ball1" );
			Blue.ECS.GameObject ball2 = RegisterGameObject( "Ball2" );

			CreateComponentData<LifetimeComponentData>( player.Id ).Life = 3000;
			CreateComponentData<LifetimeComponentData>( ball1.Id ).Life = 40;
			CreateComponentData<LifetimeComponentData>( ball2.Id ).Life = 5;

			CreateComponentData<Blue.ECS.PositionComponentData>( ball1.Id ).position = new Microsoft.Xna.Framework.Vector2( 0, 0 );
			Blue.ECS.SpriteComponentData spriteBall1 = CreateComponentData<Blue.ECS.SpriteComponentData>( ball1.Id );
			spriteBall1.name = "ball";

			CreateComponentData<Blue.ECS.PositionComponentData>( ball2.Id ).position = new Microsoft.Xna.Framework.Vector2( 200, 100 );
			Blue.ECS.SpriteComponentData spriteBall2 = CreateComponentData<Blue.ECS.SpriteComponentData>( ball2.Id );
			spriteBall2.name = "ball";
			spriteBall2.color = Microsoft.Xna.Framework.Color.Yellow;
		}
	};
}
