using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueEngine.ECS
{
	public class SpriteComponentData : IComponentData
	{
		public string GetNameId()
		{
			return "Sprite";
		}

		public String name = "";
		public Color color = Color.White;
	}

	public class SpriteComponentSystem : ComponentSystem
	{
		public static Dictionary<String, Texture2D> Textures
		{ get; set; } = new Dictionary<string, Texture2D>();

		public override string GetNameId()
		{
			return "Sprite";
		}

		public void LoadTextures()
		{
			Action<String, IComponentData> loadTexture = ( gameObjectId, data ) =>
			{
				SpriteComponentData spriteData = data as SpriteComponentData;
				Textures.Add( gameObjectId, Game.Instance.Content.Load<Texture2D>( spriteData.name ) );
			};
			ForEachData( loadTexture );
		}

		protected override void Render( String gameObjectId, IComponentData data )
		{
			SpriteComponentData spriteData = data as SpriteComponentData;
			if ( !String.IsNullOrEmpty(spriteData.name) )
			{
				// TODO Add require component
				var gameobject = scene.GetGameObject( gameObjectId );
				var positionComponent = gameobject.GetComponentData<PositionComponentData>();
				Vector2 spritePos = positionComponent.position;
				Texture2D spriteTexture = Textures[gameObjectId];
				Game.Instance.GameRenderer.PrepareToDrawSprite( spriteTexture, spritePos, spriteData.color );
			}
		}
	}
}
