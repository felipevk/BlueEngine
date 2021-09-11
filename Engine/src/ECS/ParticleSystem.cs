using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Blue.Core;

namespace Blue.ECS
{
	public enum ParticleSystemEmissorShape
	{
		Box,
		Circle
	}

	public struct Interval
	{
		public float min;
		public float max;

		public Interval( float minIn, float maxIn )
		{
			if ( minIn > maxIn )
			{
				// Assert
			}
			min = minIn;
			max = maxIn;
		}

		public float Random()
		{
			Random rand = new Random();
			float diff = max - min;
			return (float)rand.NextDouble() * diff + min;
		}

		public static float Random( float min, float max )
		{
			if ( min > max )
			{
				// Assert
			}
			Random rand = new Random();
			float diff = max - min;
			return (float)rand.NextDouble() * diff + min;
		}
	}

	public class Particle
	{
		public float currentLifetime = 0f;
		public Vector2 position = Vector2.Zero;
		public Vector2 direction = Vector2.Zero;
		public Color color = Color.White;
		public float speed;

		public void Init( ParticleComponentData particleData, Vector2 origin )
		{
			currentLifetime = particleData.lifetimeSeconds + particleData.lifetimeVariation.Random();
			direction.X = particleData.directionVariationX.Random();
			direction.Y = particleData.directionVariationY.Random();
			speed = particleData.speed.Random();
			Vector2 particlePos = origin;
			switch ( particleData.emissorShape )
			{
				case ParticleSystemEmissorShape.Box:
					particlePos.X += Interval.Random( -particleData.squareShapeWidth * 0.5f, particleData.squareShapeWidth * 0.5f );
					particlePos.Y += Interval.Random( -particleData.squareShapeHeight * 0.5f, particleData.squareShapeHeight * 0.5f );
					break;
				case ParticleSystemEmissorShape.Circle:
					Interval circleBounds = new Interval( -particleData.circleShapeRadius, particleData.circleShapeRadius );
					particlePos.X += circleBounds.Random();
					particlePos.Y += circleBounds.Random();
					break;
				default:
					break;
			}
			position = particlePos;

			Color particleColor = particleData.initialColor;
			particleColor.R += (byte)particleData.colorRVariation.Random();
			particleColor.G += (byte)particleData.colorGVariation.Random();
			particleColor.G += (byte)particleData.colorBVariation.Random();
			particleColor.B += (byte)particleData.colorAVariation.Random();
			color = particleColor;
		}

		public void Update( ParticleComponentData particleData )
		{
			Vector2 particlePos = position;
			particlePos.X += direction.X * speed * Time.DeltaTime;
			particlePos.Y += direction.Y * speed * Time.DeltaTime;

			position = particlePos;

			currentLifetime = Math.Max( currentLifetime - Time.DeltaTime, 0f );
		}

		public bool IsAlive() { return currentLifetime > 0f; }
	}

	public class ParticleComponentData : ComponentData
	{
		public ParticleSystemEmissorShape emissorShape = ParticleSystemEmissorShape.Box;
		public float circleShapeRadius = 1f;
		public float squareShapeWidth = 1f;
		public float squareShapeHeight = 1f;
		public float lifetimeSeconds = 1f;
		public Color initialColor = Color.White;
		public String spriteAssetName = "";
		public int preloadParticles = 500;
		public bool drawDebug = false;

		public Interval directionVariationX = new Interval( 0, 0 );
		public Interval directionVariationY = new Interval( 0, 0 );
		public Interval lifetimeVariation = new Interval( 0, 0 );
		public Interval colorRVariation = new Interval( 0, 0 );
		public Interval colorGVariation = new Interval( 0, 0 );
		public Interval colorBVariation = new Interval( 0, 0 );
		public Interval colorAVariation = new Interval( 0, 0 );

		public Interval particlesToEmitPerBurst = new Interval( 0, 10 );
		public Interval timeToEmit = new Interval( 0, 1000 );

		public Interval speed = new Interval( 0, 1 );
	}

	public class GameObjectParticles
	{
		private static int Max_Particles = 100;

		private int _poolIndex = 0;
		private Particle[] Particles
		{ get; set; } = new Particle[Max_Particles];

		public float _timeToEmit = 0;

		public GameObjectParticles( int particlesToPreload, Interval timeToEmit )
		{
			_timeToEmit = timeToEmit.Random();
			for ( int i = 0; i < particlesToPreload; i++ )
			{
				Particles[i] = new Particle();
			}
		}

		public void Update( ParticleComponentData particleData, Vector2 origin )
		{
			foreach ( Particle particle in Particles )
			{
				if ( particle != null && particle.IsAlive() )
					particle.Update( particleData );
			}

			_timeToEmit = Math.Max( _timeToEmit - Time.DeltaTime, 0f );

			if ( _timeToEmit == 0f )
			{
				_timeToEmit = particleData.timeToEmit.Random();
				int particlesToEmit = (int)particleData.particlesToEmitPerBurst.Random();
				for ( int i = 0; i < particlesToEmit; i++ )
				{
					Emit( particleData, origin );
				}
			}
		}

		public void Render( Texture2D particleTexture )
		{
			foreach ( Particle particle in Particles )
			{
				if ( particle != null && particle.IsAlive() )
				{
					Game.Instance.GameRenderer.PrepareToDrawSprite( particleTexture, particle.position, particle.color );
				}
			}
		}

		private void Emit( ParticleComponentData particleData, Vector2 origin )
		{
			Particle particleToEmit = Particles[_poolIndex];
			if ( particleToEmit == null )
			{
				particleToEmit = new Particle();
			}
			particleToEmit.Init( particleData, origin );
			_poolIndex = ( _poolIndex + 1 ) % Max_Particles;
		}
	}

	public class ParticleComponentSystem : ComponentSystem
	{
		private static Dictionary<String, GameObjectParticles> GameObjectParticlesInstanceMap
		{ get; set; } = new Dictionary<String, GameObjectParticles>();

		protected override void Start( string gameObjectId, ComponentData data )
		{
			ParticleComponentData particleData = data as ParticleComponentData;
			GameObject gameObject = GetGameObject( gameObjectId );

			GameObjectParticlesInstanceMap.Add( gameObjectId, new GameObjectParticles( particleData.preloadParticles, particleData.timeToEmit ) );
		}

		protected override void Update( string gameObjectId, ComponentData data )
		{
			ParticleComponentData particleData = data as ParticleComponentData;
			GameObjectParticles gameObjectParticles = GameObjectParticlesInstanceMap[gameObjectId];
			GameObject gameObject = GetGameObject( gameObjectId );
			Vector2 globalPos = new Vector2( gameObject.GetGlobalPosition().X, gameObject.GetGlobalPosition().Y );

			gameObjectParticles.Update( particleData, globalPos );
		}

		protected override void Render( String gameObjectId, ComponentData data )
		{
			ParticleComponentData particleData = data as ParticleComponentData;
			GameObjectParticles gameObjectParticles = GameObjectParticlesInstanceMap[gameObjectId];
			Texture2D particleTexture = Game.Instance.AssetManager.GetAsset<SpriteAsset>( particleData.spriteAssetName ).Texture2D;

			gameObjectParticles.Render( particleTexture );

			if ( particleData.drawDebug )
			{
				Vector3 rectPos = GetGameObject( gameObjectId ).GetGlobalPosition();

				switch ( particleData.emissorShape )
				{
					case ParticleSystemEmissorShape.Box:
						rectPos.X -= particleData.squareShapeWidth / 2;
						rectPos.Y -= particleData.squareShapeHeight / 2;
						Game.Instance.GameRenderer.PrepareToDrawRectangle( 
							new Rectangle( (int)rectPos.X, (int)rectPos.Y, (int)particleData.squareShapeWidth, (int)particleData.squareShapeHeight ),
							Color.Purple, 
							false );
						break;
					case ParticleSystemEmissorShape.Circle:
						Game.Instance.GameRenderer.PrepareToDrawCircle(
							new Vector2( rectPos.X, rectPos.Y ), 
							particleData.circleShapeRadius, 
							50,
							Color.Purple, 
							false );
						break;
				}
			}
		}
	}
}
