using Microsoft.Xna.Framework;

using Blue.ECS;

namespace Blue
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		public static Game Instance;

		public Scene CurrentScene
		{ get; set; }

		public Renderer GameRenderer
		{ get; }

		public static void Run<T>()
			where T : Game, new()
		{
			var factory = new MonoGame.Framework.GameFrameworkViewSource<T>();
			Windows.ApplicationModel.Core.CoreApplication.Run( factory );
		}

		public Game()
        {
			Instance = this;
			GameRenderer = new Renderer();
			Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
			Log.Message( "BlueEngine Game Started!" );
			base.Initialize();
			CurrentScene.Start();
		}

        protected override void LoadContent()
        {
			GameRenderer.LoadContent();
			CurrentScene.LoadContent();
		}

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			CurrentScene.Update();
		}

        protected override void Draw(GameTime gameTime)
        {
			CurrentScene.Render();
			GameRenderer.Render( gameTime );

			base.Draw(gameTime);
        }
    }
}
