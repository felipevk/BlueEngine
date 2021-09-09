using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Blue.ECS
{
	public class SpriteComponentData : ComponentData
	{
		public String name = "";
		public Color color = Color.White;
		public Color drawDebugColor = Color.Red;
		public bool isVisible = true;
		public bool drawDebug = false;
	}

	public class SpriteComponentSystem : ComponentSystem
	{
		private static Dictionary<String, Texture2D> Textures
		{ get; set; } = new Dictionary<String, Texture2D>();

		private static Dictionary<String, String> GameObjectTextureMap
		{ get; set; } = new Dictionary<String, String>();

		public override void LoadContent()
		{
			Action<String, ComponentData> loadTexture = ( gameObjectId, data ) =>
			{
				SpriteComponentData spriteData = data as SpriteComponentData;
				if ( !Textures.ContainsKey(spriteData.name) && !String.IsNullOrEmpty( spriteData.name ) )
				{
					Textures.Add( spriteData.name, Game.Instance.Content.Load<Texture2D>( spriteData.name ) );
				}
				GameObjectTextureMap.Add( gameObjectId , spriteData.name );
			};
			ForEachData( loadTexture );
		}

		protected override void Render( String gameObjectId, ComponentData data )
		{
			SpriteComponentData spriteData = data as SpriteComponentData;
			if ( !String.IsNullOrEmpty(spriteData.name) )
			{
				Texture2D spriteTexture = Textures[GameObjectTextureMap[gameObjectId]];
				Vector3 spritePos = scene.GetGameObject( gameObjectId ).GetGlobalPosition();
				spritePos.X -= spriteTexture.Width / 2;
				spritePos.Y -= spriteTexture.Height / 2;
				if ( spriteData.isVisible )
				{
					Game.Instance.GameRenderer.PrepareToDrawSprite( spriteTexture, new Vector2( spritePos.X, spritePos.Y ), spriteData.color );
				}
				if ( spriteData.drawDebug )
				{
					Game.Instance.GameRenderer.PrepareToDrawRectangle( 
						new Rectangle( (int)spritePos.X, (int)spritePos.Y, spriteTexture.Width, spriteTexture.Height ),
						spriteData.drawDebugColor,
						false );
				}
			}
		}
	}
}
