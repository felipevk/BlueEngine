using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.ECS
{
	public class SpriteComponentData : IComponentData
	{
		public String name = "";
		public Color color = Color.White;
	}

	public class SpriteComponentSystem : ComponentSystem
	{
		private static Dictionary<String, Texture2D> Textures
		{ get; set; } = new Dictionary<String, Texture2D>();

		private static Dictionary<String, String> GameObjectTextureMap
		{ get; set; } = new Dictionary<String, String>();

		public void LoadTextures()
		{
			Action<String, IComponentData> loadTexture = ( gameObjectId, data ) =>
			{
				SpriteComponentData spriteData = data as SpriteComponentData;
				if ( !Textures.ContainsKey(spriteData.name) )
				{
					Textures.Add( spriteData.name, Game.Instance.Content.Load<Texture2D>( spriteData.name ) );
				}
				GameObjectTextureMap.Add( gameObjectId , spriteData.name );
			};
			ForEachData( loadTexture );
		}

		protected override void Render( String gameObjectId, IComponentData data )
		{
			SpriteComponentData spriteData = data as SpriteComponentData;
			if ( !String.IsNullOrEmpty(spriteData.name) )
			{
				// TODO Add require component
				Vector2 spritePos = Scene.GetComponentData<PositionComponentData>( gameObjectId ).position;
				Texture2D spriteTexture = Textures[GameObjectTextureMap[gameObjectId]];
				Game.Instance.GameRenderer.PrepareToDrawSprite( spriteTexture, spritePos, spriteData.color );
			}
		}
	}
}
