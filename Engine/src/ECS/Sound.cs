using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

using Blue.Core;

namespace Blue.ECS
{
	public class SoundComponentData : ComponentData
	{
		public String assetName = "";
		public float volume = 1.0f;
		public float pitch = 0.0f;
		public float pan = 0.0f;
		public bool loop = false;
		public bool isPlaying = false;
	}

	public class SoundComponentSystem : ComponentSystem
	{
		private static Dictionary<String, SoundEffectInstance> GameObjectSoundEffectInstanceMap
		{ get; set; } = new Dictionary<String, SoundEffectInstance>();

		protected override void Update( String gameObjectId, ComponentData data )
		{
			SoundComponentData soundData = data as SoundComponentData;
			if ( GameObjectSoundEffectInstanceMap.ContainsKey( gameObjectId ) )
			{
				SoundEffectInstance soundEffectInstance = GameObjectSoundEffectInstanceMap[gameObjectId];

				soundEffectInstance.Volume = soundData.volume;
				soundEffectInstance.Pitch = soundData.pitch;
				soundEffectInstance.Pan = soundData.pan;
				soundEffectInstance.IsLooped = soundData.loop;
			}
		}

		public static SoundEffectInstance CreateOrAddSoundEffectInstance( String gameObjectId, SoundComponentData data )
		{
			SoundEffectInstance soundEffectInstance;
			if ( GameObjectSoundEffectInstanceMap.ContainsKey( gameObjectId ) )
			{
				soundEffectInstance = GameObjectSoundEffectInstanceMap[gameObjectId];
			}
			else if( Game.Instance.AssetManager.HasAsset<SoundEffectAsset>( data.assetName ) )
			{
				soundEffectInstance = Game.Instance.AssetManager.GetAsset<SoundEffectAsset>( data.assetName ).SoundEffect.CreateInstance();
				GameObjectSoundEffectInstanceMap.Add( gameObjectId, soundEffectInstance );
			}
			else
			{
				// Assert
				return null;
			}

			soundEffectInstance.Volume = data.volume;
			soundEffectInstance.Pitch = data.pitch;
			soundEffectInstance.Pan = data.pan;
			soundEffectInstance.IsLooped = data.loop;

			return soundEffectInstance;
		}

		public static void PlayOnce( String assetName, float volume = 1.0f )
		{
			SoundEffectInstance soundEffectInstance = Game.Instance.AssetManager.GetAsset<SoundEffectAsset>( assetName ).SoundEffect.CreateInstance();
			soundEffectInstance.Volume = volume;

			soundEffectInstance.Play();
		}

		public static void PlaySoundEffect( String gameObjectId, SoundComponentData soundData )
		{
			soundData.isPlaying = true;
			// TODO set isPlaying to false when sound ends

			SoundEffectInstance soundEffectInstance = CreateOrAddSoundEffectInstance( gameObjectId, soundData );
			soundEffectInstance.Play();
		}

		public static void PauseSoundEffect( String gameObjectId, SoundComponentData soundData )
		{
			soundData.isPlaying = false;

			SoundEffectInstance soundEffectInstance = CreateOrAddSoundEffectInstance( gameObjectId, soundData );
			soundEffectInstance.Pause();
		}

		public static void StopSoundEffect( String gameObjectId, SoundComponentData soundData )
		{
			soundData.isPlaying = false;

			SoundEffectInstance soundEffectInstance = CreateOrAddSoundEffectInstance( gameObjectId, soundData );
			soundEffectInstance.Stop();
		}

		public static void PlaySong( SoundComponentData soundData )
		{
			soundData.isPlaying = true;
			// TODO set isPlaying to false when sound ends

			SongAsset songAsset = Game.Instance.AssetManager.GetAsset<SongAsset>( soundData.assetName );
			MediaPlayer.Volume = soundData.volume;
			MediaPlayer.IsRepeating = soundData.loop;
			MediaPlayer.Play( songAsset.Song );
		}

		public static void PauseSong( SoundComponentData soundData )
		{
			soundData.isPlaying = false;
			MediaPlayer.Pause();
		}

		public static void StopSong( SoundComponentData soundData )
		{
			soundData.isPlaying = false;
			MediaPlayer.Stop();
		}
		public static void SetSongVolume( float volume )
		{
			MediaPlayer.Volume = volume;
		}
	}
}
