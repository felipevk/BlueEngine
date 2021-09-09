using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace Blue.ECS
{
	public class SoundComponentData : ComponentData
	{
		public String name = "";
		public float volume = 1.0f;
		public float pitch = 0.0f;
		public float pan = 0.0f;
		public bool loop = false;
	}

	public class SoundComponentSystem : ComponentSystem
	{
		private static Dictionary<String, SoundEffect> SoundEffects
		{ get; set; } = new Dictionary<String, SoundEffect>();

		private static Dictionary<String, SoundEffectInstance> GameObjectSoundEffectInstanceMap
		{ get; set; } = new Dictionary<String, SoundEffectInstance>();

		public override void LoadContent()
		{
			Action<String, ComponentData> loadSoundEffect = ( gameObjectId, data ) =>
			{
				SoundComponentData soundData = data as SoundComponentData;
				if ( !SoundEffects.ContainsKey( soundData.name ) )
				{
					SoundEffects.Add( soundData.name, Game.Instance.Content.Load<SoundEffect>( soundData.name ) );
				}
			};
			ForEachData( loadSoundEffect );
		}

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
			else
			{
				soundEffectInstance = SoundEffects[data.name].CreateInstance();
				GameObjectSoundEffectInstanceMap.Add( gameObjectId, soundEffectInstance );
			}

			soundEffectInstance.Volume = data.volume;
			soundEffectInstance.Pitch = data.pitch;
			soundEffectInstance.Pan = data.pan;
			soundEffectInstance.IsLooped = data.loop;

			return soundEffectInstance;
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
