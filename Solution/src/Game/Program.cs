﻿using Microsoft.Xna.Framework;

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
		Blue.Game.Run<MyGame>();
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

			Blue.ECS.SpriteComponentData spriteBall1 = CreateComponentData<Blue.ECS.SpriteComponentData>( ball1.Id );
			spriteBall1.name = "ball";
			spriteBall1.drawDebug = true;

			ball2.Transform.Position = new Vector3( 200, 100, 0 );
			Blue.ECS.SpriteComponentData spriteBall2 = CreateComponentData<Blue.ECS.SpriteComponentData>( ball2.Id );
			spriteBall2.name = "ball";
			spriteBall2.color = Color.Yellow;
		}
	};
}
