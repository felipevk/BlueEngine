using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BlueEngine
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

		protected GraphicsDeviceManager _graphics;
		protected SpriteBatch _spriteBatch;

		private List<SpriteDrawCall> _spriteDrawCalls = new List<SpriteDrawCall>();

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

			_spriteBatch.Begin();
			foreach ( SpriteDrawCall spriteDrawCall in _spriteDrawCalls )
			{
				_spriteBatch.Draw( spriteDrawCall.texture, spriteDrawCall.position, spriteDrawCall.color );
			}
			_spriteBatch.End();
			_spriteDrawCalls.Clear();
		}

		public void PrepareToDrawSprite( Texture2D texture, Vector2 position, Color color )
		{
			_spriteDrawCalls.Add( new SpriteDrawCall( texture, position, color ) );
		}
	}
}
