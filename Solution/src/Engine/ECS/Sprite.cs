using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using Blue.Core;

namespace Blue.ECS
{
	public class SpriteComponentData : ComponentData
	{
		public String assetName = "";
		public Color color = Color.White;
		public Color drawDebugColor = Color.Red;
		public bool isVisible = true;
		public bool drawDebug = false;
	}

	public class SpriteComponentSystem : ComponentSystem
	{
		protected override void Render( String gameObjectId, ComponentData data )
		{
			SpriteComponentData spriteData = data as SpriteComponentData;
			if ( !String.IsNullOrEmpty(spriteData.assetName) && Game.Instance.AssetManager.HasAsset<SpriteAsset>( spriteData.assetName ) )
			{
				Texture2D spriteTexture = Game.Instance.AssetManager.GetAsset<SpriteAsset>( spriteData.assetName ).Texture2D;
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
