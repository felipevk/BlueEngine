using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using Blue.Core;

namespace Blue.ECS
{
	public class TextComponentData : ComponentData
	{
		public String assetName = "";
		public String text = "";
		public Color color = Color.White;
		public bool isVisible = true;
	}

	public class TextComponentSystem : ComponentSystem
	{

		protected override void Render( String gameObjectId, ComponentData data )
		{
			TextComponentData textData = data as TextComponentData;
			if ( textData.isVisible || 
				!String.IsNullOrEmpty( textData.assetName ) || 
				!String.IsNullOrEmpty( textData.text ) ||
				 Game.Instance.AssetManager.HasAsset<FontAsset>( textData.assetName ) )
			{
				SpriteFont font = Game.Instance.AssetManager.GetAsset<FontAsset>( textData.assetName ).SpriteFont;
				Vector3 textPos = scene.GetGameObject( gameObjectId ).GetGlobalPosition();
				Game.Instance.GameRenderer.PrepareToDrawText( font, textData.text, new Vector2( textPos.X, textPos.Y ), textData.color );
			}
		}
	}
}
