using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Blue.Core;
using Blue.ECS;

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

			Input.CreateInputGroup( "fire1" );
			Input.AddButtonToGroup( "fire1", Buttons.RightTrigger );
			Input.AddKeyToGroup( "fire1", Keys.Space );
			Input.AddKeyToGroup( "fire1", Keys.Enter );
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
			Blue.ECS.GameObject ball3 = RegisterGameObject( "Ball3" );

			ball1.AddChild( ball3 );

			CreateComponentData<LifetimeComponentData>( player.Id ).Life = 3000;
			CreateComponentData<LifetimeComponentData>( ball1.Id ).Life = 40;
			CreateComponentData<LifetimeComponentData>( ball2.Id ).Life = 5;

			CreateComponentData<SoundComponentData>( ball1.Id ).name = "pop";
			CreateComponentData<SoundComponentData>( ball2.Id ).name = "pop";

			SpriteComponentData spriteBall1 = CreateComponentData<Blue.ECS.SpriteComponentData>( ball1.Id );
			spriteBall1.name = "ball";
			spriteBall1.drawDebug = true;

			ball2.Transform.Position = new Vector3( 200, 100, 0 );
			SpriteComponentData spriteBall2 = CreateComponentData<Blue.ECS.SpriteComponentData>( ball2.Id );
			spriteBall2.name = "ball";
			spriteBall2.color = Color.Yellow;

			ball3.Transform.Position = new Vector3( 10, 30, 0 );
			SpriteComponentData spriteBall3 = CreateComponentData<Blue.ECS.SpriteComponentData>( ball3.Id );
			spriteBall3.name = "ball";
		}
	};
}
