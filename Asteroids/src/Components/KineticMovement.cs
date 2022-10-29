using System;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Components
{
	internal class KineticMovement : Component
	{
		public struct Options
		{
			public float MaxSpeed;
			public float Acceleration;
			public float Deceleration;
		}

		private readonly IKeyStateProvider keys;
		private readonly Options options;

		private float speed;

		public KineticMovement(
			IGameObject owner, IKeyStateProvider keyStateProvider, Options speedOptions
		) : base(
			owner
		) {
			keys = keyStateProvider;
			options = speedOptions;
		}

		public override void Update(GameTime gameTime)
		{
			float deltaSec = gameTime.ElapsedSeconds();
			float desiredSpeed = keys.IsPressed(Keys.Up) ? options.MaxSpeed : 0;

			float addSpeed = speed > desiredSpeed
				? -options.Deceleration * deltaSec
				: options.Acceleration * deltaSec;

			var (sin, cos) = MathF.SinCos(Owner.Rotation);
			speed = Math.Clamp(speed + addSpeed, 0, options.MaxSpeed);
			Owner.Position += new Vector2(cos, sin) * speed * deltaSec;
		}
	}
}
