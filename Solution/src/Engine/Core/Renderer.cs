using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System.Collections.Generic;

namespace Blue
{
	public class Renderer
	{
		struct SpriteDrawCall
		{
			public Texture2D texture;
			public Vector2 position;
			public Color color;

			public SpriteDrawCall( Texture2D textureIn, Vector2 positionIn, Color colorIn )
			{
				texture = textureIn;
				position = positionIn;
				color = colorIn;
			}
		}

		struct RectangleDrawCall
		{
			public Rectangle rect;
			public Color color;

			public RectangleDrawCall( Rectangle rectIn, Color colorIn )
			{
				rect = rectIn;
				color = colorIn;
			}
		}

		protected GraphicsDeviceManager _graphics;
		protected SpriteBatch _spriteBatch;

		private List<SpriteDrawCall> _spriteDrawCalls = new List<SpriteDrawCall>();
		private List<RectangleDrawCall> _rectangleDrawCalls = new List<RectangleDrawCall>();

		public Renderer()
		{
			_graphics = new GraphicsDeviceManager( Game.Instance );
		}

		public void LoadContent()
		{
			_spriteBatch = new SpriteBatch( Game.Instance.GraphicsDevice );
		}

		public void Render( GameTime gameTime )
		{
			Game.Instance.GraphicsDevice.Clear( Color.CornflowerBlue );

			Game.Instance.CurrentScene.Render();

			_spriteBatch.Begin();
			foreach ( SpriteDrawCall spriteDrawCall in _spriteDrawCalls )
			{
				_spriteBatch.Draw( spriteDrawCall.texture, spriteDrawCall.position, spriteDrawCall.color );
			}
			foreach ( RectangleDrawCall rectangleDrawCall in _rectangleDrawCalls )
			{
				Primitives2D.DrawRectangle( _spriteBatch, rectangleDrawCall.rect, rectangleDrawCall.color );
			}
			_spriteBatch.End();
			_spriteDrawCalls.Clear();
			_rectangleDrawCalls.Clear();
		}

		public void PrepareToDrawSprite( Texture2D texture, Vector2 position, Color color )
		{
			_spriteDrawCalls.Add( new SpriteDrawCall( texture, position, color ) );
		}

		public void PrepareToDrawRectangle( Rectangle rect, Color color )
		{
			_rectangleDrawCalls.Add( new RectangleDrawCall( rect, color ) );
		}
	}
}
