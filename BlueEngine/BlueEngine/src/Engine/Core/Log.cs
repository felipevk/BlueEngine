using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueEngine
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
