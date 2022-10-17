using System;
using Asteroids.Core;
using Asteroids.Utils;
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

		private readonly IGameObject owner;
		private readonly IKeyStateProvider keys;
		private readonly Options options;

		private float speed;

		private Vector2 OwnerDirection => new Vector2(
			MathF.Cos(owner.Rotation), MathF.Sin(owner.Rotation)
		);

		public KineticMovement(
			IGameObject gameObject, IKeyStateProvider keyStateProvider, Options speedOptions
		)
		{
			owner = gameObject;
			keys = keyStateProvider;
			options = speedOptions;
		}

		public void Update(GameTime gameTime)
		{
			float deltaSec = gameTime.ElapsedSeconds();
			float desiredSpeed = keys.IsPressed(Keys.Up) ? options.MaxSpeed : 0;

			float addSpeed = speed > desiredSpeed
				? -options.Deceleration * deltaSec
				: options.Acceleration * deltaSec;

			speed = Math.Clamp(speed + addSpeed, 0, options.MaxSpeed);
			owner.Position += OwnerDirection * speed * deltaSec;
		}
	}
}
