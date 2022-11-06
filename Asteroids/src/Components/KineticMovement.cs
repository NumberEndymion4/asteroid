using System;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Components
{
	internal class KineticMovement : IComponent
	{
		public struct Settings
		{
			public float MaxSpeed;
			public float Acceleration;
			public float Deceleration;
		}

		private readonly IKeyStateProvider keys;
		private readonly Settings settings;

		private float speed;

		public KineticMovement(IKeyStateProvider keyStateProvider, Settings speedSettings)
		{
			keys = keyStateProvider;
			settings = speedSettings;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			float deltaSec = gameTime.ElapsedSeconds();
			float desiredSpeed = keys.IsPressed(Keys.Up) ? settings.MaxSpeed : 0;

			float addSpeed = speed > desiredSpeed
				? -settings.Deceleration * deltaSec
				: settings.Acceleration * deltaSec;

			var (sin, cos) = MathF.SinCos(gameObject.Rotation);
			speed = Math.Clamp(speed + addSpeed, 0, settings.MaxSpeed);
			gameObject.Position += new Vector2(cos, sin) * speed * deltaSec;
		}
	}
}
