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
			public bool isFilled;

			public RectangleDrawCall( Rectangle rectIn, Color colorIn, bool isFilledIn )
			{
				rect = rectIn;
				color = colorIn;
				isFilled = isFilledIn;
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

		public void SetWindowSize( int width, int height )
		{
			_graphics.PreferredBackBufferWidth = width;
			_graphics.PreferredBackBufferHeight = height;
			_graphics.ApplyChanges();
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
				if ( rectangleDrawCall.isFilled )
					Primitives2D.FillRectangle( _spriteBatch, rectangleDrawCall.rect, rectangleDrawCall.color );
				else
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

		public void PrepareToDrawRectangle( Rectangle rect, Color color, bool isFilled )
		{
			_rectangleDrawCalls.Add( new RectangleDrawCall( rect, color, isFilled ) );
		}
	}
}
