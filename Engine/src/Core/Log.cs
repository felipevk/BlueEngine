using System;
using System.Diagnostics;

namespace Blue
{
	public static class Log
	{
		public static void Message( String message )
		{
			Debug.WriteLine( message );
		}

		public static void Error( String message )
		{
			Debug.WriteLine( message );
		}

		public static void Warning( String message )
		{
			Debug.WriteLine( message );
		}
	}
}
