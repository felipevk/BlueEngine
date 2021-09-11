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

			AssetManager.AddAsset<SpriteAsset>( "ball" );
			AssetManager.AddAsset<SoundEffectAsset>( "pop" );
			AssetManager.AddAsset<FontAsset>( "PixeloidSans" );
		}
	}
	public class MainScene : Scene
	{
		protected override void RegisterComponents()
		{
			base.RegisterComponents();

			RegisterComponent<LifetimeComponentSystem, LifetimeComponentData>();
		}

		protected override void RegisterGameObjects()
		{
			base.RegisterGameObjects();

			GameObject player = CreateGameObject( "Player" );
			GameObject ball1 = CreateGameObject( "Ball1" );
			GameObject ball2 = CreateGameObject( "Ball2" );
			GameObject ball3 = CreateGameObject( "Ball3" );

			ball1.AddChild( ball3 );

			CreateComponentData<LifetimeComponentData>( player.Id ).Life = 3000;
			CreateComponentData<LifetimeComponentData>( ball1.Id ).Life = 400;
			CreateComponentData<LifetimeComponentData>( ball2.Id ).Life = 0;

			CreateComponentData<SoundComponentData>( ball1.Id ).assetName = "pop";
			CreateComponentData<SoundComponentData>( ball2.Id ).assetName = "pop";

			ball1.Transform.Position = new Vector3( 200, 0, 0 );

			//SpriteComponentData was added from LifetimeComponentData
			SpriteComponentData spriteBall1 = GetComponentData<SpriteComponentData>( ball1.Id );
			spriteBall1.assetName = "ball";
			spriteBall1.drawDebug = true;

			TextComponentData ball1Text = CreateComponentData<TextComponentData>( ball1.Id );
			ball1Text.assetName = "PixeloidSans";
			ball1Text.text = "Hello World!";

			BoxCollision2DComponentData ball1Collider = CreateComponentData<BoxCollision2DComponentData>( ball1.Id );
			ball1Collider.drawDebug = true;
			ball1Collider.Width = 40;
			ball1Collider.Height = 40;

			ball2.Transform.Position = new Vector3( 400, 200, 0 );

			//SpriteComponentData was added from LifetimeComponentData
			SpriteComponentData spriteBall2 = GetComponentData<SpriteComponentData>( ball2.Id );
			spriteBall2.assetName = "ball";
			spriteBall2.color = Color.Yellow;

			BoxCollision2DComponentData ball2Collider = CreateComponentData<BoxCollision2DComponentData>( ball2.Id );
			ball2Collider.drawDebug = true;
			ball2Collider.Width = 100;
			ball2Collider.Height = 100;

			TextComponentData ball2Text = CreateComponentData<TextComponentData>( ball2.Id );
			ball2Text.assetName = "PixeloidSans";
			ball2Text.text = "In Different Colors";
			ball2Text.color = Color.Green;

			ball3.Transform.Position = new Vector3( 10, 30, 0 );
			SpriteComponentData spriteBall3 = CreateComponentData<SpriteComponentData>( ball3.Id );
			spriteBall3.assetName = "ball";

			BoxCollision2DComponentData ball3Collider = CreateComponentData<BoxCollision2DComponentData>( ball3.Id );
			ball3Collider.drawDebug = true;
			ball3Collider.Width = 40;
			ball3Collider.Height = 40;
		}
	};
}
