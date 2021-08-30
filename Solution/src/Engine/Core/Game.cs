using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using BlueEngine.ECS;

namespace BlueEngine
{
    public class Game : Microsoft.Xna.Framework.Game
	{
		public Scene CurrentScene
		{ get; set; }

		private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

		public Game()
        {
			_graphics = new GraphicsDeviceManager(this);
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            base.Update(gameTime);

			CurrentScene.Update();
			CurrentScene.Render();
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
