using System;
using Asteroids.Core;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer.Behaviors
{
	public class AngleProvider : IBehavior, IDataProvider<float>
	{
		public float Data { get; private set; }

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			// MonoGame wraps angles in the range from 0 to PI so that
			// positive values are in the lower semicircle (CW) and
			// negative values are in the upper one (CCW).
			float angle = gameObject.Rotation;
			Data = MathHelper.ToDegrees((angle > 0f ? MathF.Tau : 0f) - angle);
		}
	}
}
