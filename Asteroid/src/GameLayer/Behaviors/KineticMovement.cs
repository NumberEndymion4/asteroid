using System;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.GameLayer.Behaviors
{
	public class KineticMovement : IBehavior
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

		public KineticMovement(IKeyStateProvider keyStateProvider, Options speedOptions)
		{
			keys = keyStateProvider;
			options = speedOptions;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			float deltaSec = gameTime.ElapsedSeconds();
			float desiredSpeed = keys.IsPressed(Keys.Up) ? options.MaxSpeed : 0;

			float addSpeed = speed > desiredSpeed
				? -options.Deceleration * deltaSec
				: options.Acceleration * deltaSec;

			var (sin, cos) = MathF.SinCos(gameObject.Rotation);
			speed = Math.Clamp(speed + addSpeed, 0, options.MaxSpeed);
			gameObject.Position += new Vector2(cos, sin) * speed * deltaSec;
		}
	}
}
