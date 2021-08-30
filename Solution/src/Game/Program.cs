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
		Microsoft.Xna.Framework.Graphics.Texture2D ballTexture;

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

			ball1.AddComponentData<BlueEngine.ECS.PositionComponentData>().position = new Microsoft.Xna.Framework.Vector2( 0, 0 );
			BlueEngine.ECS.SpriteComponentData spriteBall1 = ball1.AddComponentData<BlueEngine.ECS.SpriteComponentData>();
			spriteBall1.name = "ball";


			ball2.AddComponentData<BlueEngine.ECS.PositionComponentData>().position = new Microsoft.Xna.Framework.Vector2( 200, 100 );
			BlueEngine.ECS.SpriteComponentData spriteBall2 = ball2.AddComponentData<BlueEngine.ECS.SpriteComponentData>();
			spriteBall2.name = "ball";
			spriteBall2.color = Microsoft.Xna.Framework.Color.Yellow;
		}
	};
}
