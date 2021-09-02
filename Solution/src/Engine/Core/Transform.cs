﻿using Microsoft.Xna.Framework;

namespace Blue.Core
{
	public class Transform
	{
		public Vector3 Position
		{ get; set; } = Vector3.Zero;

		public Vector3 Scale
		{ get; set; }

		public Quaternion Rotation
		{ get; set; }
	}
}
