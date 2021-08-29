using System;

using BlueEngine.ECS;

namespace BlueEngine
{
	public class Game
	{
		public bool IsRunning
		{ get; set; }

		public Scene CurrentScene
		{ get; set; }

		public Game()
		{
			Console.WriteLine("BlueEngine Game Started!");
			IsRunning = true;
		}

		public void Run()
		{
			CurrentScene.Start();
			GameLoop();
			Exit();
		}

		public void Exit()
		{
			Console.WriteLine( "BlueEngine Game Finished!" );
		}

		private void GameLoop()
		{
			while ( IsRunning )
			{
				System.Threading.Thread.Sleep( 2000 );
				Console.WriteLine( "----------New Loop----------" );
				CurrentScene.Update();
				CurrentScene.Render();
			}
		}
	}
}
