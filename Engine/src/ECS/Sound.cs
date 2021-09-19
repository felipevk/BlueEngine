using Microsoft.Xna.Framework.Audio;
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

		public static void Play( String gameObjectId, SoundComponentData data )
		{
			SoundEffectInstance soundEffectInstance = CreateOrAddSoundEffectInstance( gameObjectId, data );

			soundEffectInstance.Play();
		}

		public static void Pause( String gameObjectId, SoundComponentData data )
		{
			SoundEffectInstance soundEffectInstance = CreateOrAddSoundEffectInstance( gameObjectId, data );

			soundEffectInstance.Pause();
		}

		public static void Stop( String gameObjectId, SoundComponentData data )
		{
			SoundEffectInstance soundEffectInstance = CreateOrAddSoundEffectInstance( gameObjectId, data );

			soundEffectInstance.Stop();
		}
	}
}
