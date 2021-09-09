using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Blue.ECS
{
	public class TextComponentData : ComponentData
	{
		public String fontName = "";
		public String text = "";
		public Color color = Color.White;
		public bool isVisible = true;
	}

	public class TextComponentSystem : ComponentSystem
	{
		private static Dictionary<String, SpriteFont> Fonts
		{ get; set; } = new Dictionary<String, SpriteFont>();

		private static Dictionary<String, String> GameObjectFontMap
		{ get; set; } = new Dictionary<String, String>();

		public override void LoadContent()
		{
			Action<String, ComponentData> loadFont = ( gameObjectId, data ) =>
			{
				TextComponentData textData = data as TextComponentData;
				if ( !Fonts.ContainsKey( textData.fontName ) )
				{
					Fonts.Add( textData.fontName, Game.Instance.Content.Load<SpriteFont>( textData.fontName ) );
				}
				GameObjectFontMap.Add( gameObjectId, textData.fontName );
			};
			ForEachData( loadFont );
		}

		protected override void Render( String gameObjectId, ComponentData data )
		{
			TextComponentData textData = data as TextComponentData;
			if ( textData.isVisible || !String.IsNullOrEmpty( textData.fontName ) || !String.IsNullOrEmpty( textData.text ) )
			{
				SpriteFont font = Fonts[GameObjectFontMap[gameObjectId]];
				Vector3 textPos = scene.GetGameObject( gameObjectId ).GetGlobalPosition();
				Game.Instance.GameRenderer.PrepareToDrawText( font, textData.text, new Vector2( textPos.X, textPos.Y ), textData.color );
			}
		}
	}
}
