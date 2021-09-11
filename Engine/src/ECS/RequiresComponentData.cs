using System;

namespace Blue.ECS
{
	[AttributeUsage( System.AttributeTargets.Class,
					   AllowMultiple = true )]
	public class RequiresComponentData : Attribute
	{
		public Type required;
		public RequiresComponentData( Type requiredIn ) => required = requiredIn;
	}
}
