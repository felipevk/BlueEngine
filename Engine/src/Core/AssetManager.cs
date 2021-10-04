using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Blue.Core
{
	public interface IAsset
	{
		void LoadContent( string assetName );
	}

	public class SpriteAsset : IAsset
	{
		public Texture2D Texture2D
		{ get; set; }

		public void LoadContent( string assetName )
		{
			Texture2D = Game.Instance.Content.Load<Texture2D>( assetName );
		}
	}

	public class SoundEffectAsset : IAsset
	{
		public SoundEffect SoundEffect
		{ get; set; }

		public void LoadContent( string assetName )
		{
			SoundEffect = Game.Instance.Content.Load<SoundEffect>( assetName );
		}
	}

	public class SongAsset : IAsset
	{
		public Song Song
		{ get; set; }

		public void LoadContent( string assetName )
		{
			Song = Game.Instance.Content.Load<Song>( assetName );
		}
	}

	public class FontAsset : IAsset
	{
		public SpriteFont SpriteFont
		{ get; set; }

		public void LoadContent( string assetName )
		{
			SpriteFont = Game.Instance.Content.Load<SpriteFont>( assetName );
		}
	}

	public class AssetManager
	{
		private Dictionary<String, Dictionary<String, IAsset>> Assets
		{ get; set; } = new Dictionary<string, Dictionary<string, IAsset>>();

		public void RegisterAssetType<T>()
			where T : IAsset
		{
			Assets.TryAdd( typeof( T ).ToString(), new Dictionary<string, IAsset>() );
		}

		public void AddAsset<T>( String assetName )
			where T : IAsset, new()
		{
			if ( HasAsset<T>( assetName ) )
				return;

			T newAsset = new T();
			newAsset.LoadContent( assetName );
			
			Assets[typeof( T ).ToString()].Add( assetName, newAsset );
		}

		public bool HasAsset<T>( String assetName )
			where T : IAsset
		{
			return Assets[typeof( T ).ToString()].ContainsKey( assetName );
		}

		public T GetAsset<T>( String assetName )
		where T : IAsset
		{
			if ( HasAsset<T>( assetName ) )
			{
				return (T)Assets[typeof( T ).ToString()][assetName];
			}
			else
			{
				// TODO assert
				return default( T );
			}
		}
	}
}
