using System;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class AngleProvider : Component, IDataProvider<float>
	{
		public float Data { get; private set; }

		public AngleProvider(IGameObject owner) : base(owner)
		{
		}

		public override void Update(GameTime gameTime)
		{
			// MonoGame wraps angles in the range from 0 to PI so that
			// positive values are in the lower semicircle (CW) and
			// negative values are in the upper one (CCW).
			float angle = Owner.Rotation;
			Data = MathHelper.ToDegrees((angle > 0f ? MathF.Tau : 0f) - angle);
		}
	}
}
