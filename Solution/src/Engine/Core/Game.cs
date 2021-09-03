using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Blue.ECS;
using Blue.Core;

namespace Blue
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		public static Game Instance;

		public Scene CurrentScene
		{ get; set; }

		public Renderer GameRenderer
		{ get; }

		private static Dictionary<String, ManagedSystem> m_managedSystems = new Dictionary<string, ManagedSystem>();

		public static void Run<T>()
			where T : Game, new()
		{
			var factory = new MonoGame.Framework.GameFrameworkViewSource<T>();
			Windows.ApplicationModel.Core.CoreApplication.Run( factory );
		}

		public Game()
		{
			Instance = this;
			GameRenderer = new Renderer();
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			RegisterManagedSystems();
		}

		protected override void Initialize()
		{
			Log.Message( "BlueEngine Game Started!" );
			base.Initialize();
			foreach ( KeyValuePair<String, ManagedSystem> entry in m_managedSystems )
			{
				ManagedSystem system = entry.Value;
				system.Start();
			}
			CurrentScene.Start();
		}

		protected override void LoadContent()
		{
			GameRenderer.LoadContent();
			CurrentScene.LoadContent();
		}

		protected virtual void RegisterManagedSystems()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Input.Update();

			foreach ( KeyValuePair<String, ManagedSystem> entry in m_managedSystems )
			{
				ManagedSystem system = entry.Value;
				system.Update();
			}
			CurrentScene.Update();

			Collision2DManager.Update();

			CurrentScene.ProcessCollisions( Collision2DManager.GetGlobalState() );
		}

		protected override void Draw(GameTime gameTime)
		{
			foreach ( KeyValuePair<String, ManagedSystem> entry in m_managedSystems )
			{
				ManagedSystem system = entry.Value;
				system.Render();
			}
			GameRenderer.Render( gameTime );

			base.Draw(gameTime);
		}

		protected void RegisterManagedSystem<T>()
			where T : ManagedSystem, new()
		{
			if ( !m_managedSystems.ContainsKey( typeof( T ).ToString() ) )
			{
				T system = new T();
				m_managedSystems.Add( typeof( T ).ToString(), system );
			}
			else
			{
				// TODO Assert
			}
		}

		public static bool HasManagedSystem<T>()
			where T : ManagedSystem
		{
			return m_managedSystems.ContainsKey( typeof( T ).ToString() );
		}

		public static T GetManagedSystem<T>()
			where T : ManagedSystem
		{
			if ( HasManagedSystem<T>() )
			{
				return (T)m_managedSystems[typeof( T ).ToString()];
			}
			else
			{
				// TODO assert
				return default( T );
			}
		}
	}
}
