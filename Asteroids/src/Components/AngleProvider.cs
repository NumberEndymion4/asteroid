using System;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class AngleProvider : IDataProvider<float>
	{
		public float Radians { get; private set; }
		public float Degrees { get; private set; }

		float IDataProvider<float>.Data => Degrees;

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			// MonoGame wraps angles in the range from 0 to PI so that
			// positive values are in the lower semicircle (CW) and
			// negative values are in the upper one (CCW).
			Radians = gameObject.Rotation;
			Degrees = MathHelper.ToDegrees((Radians > 0f ? MathF.Tau : 0f) - Radians);
		}
	}
}
