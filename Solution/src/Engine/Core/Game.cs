using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using BlueEngine.ECS;

namespace BlueEngine
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		public static Game Instance;

		public Scene CurrentScene
		{ get; set; }

		public Renderer GameRenderer
		{ get; }

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
