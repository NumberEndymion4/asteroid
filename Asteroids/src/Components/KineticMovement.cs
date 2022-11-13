using System;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class KineticMovement : Disposable, IComponent, IBroadcastListener
	{
		public struct Settings
		{
			public float MaxSpeed;
			public float Acceleration;
			public float Deceleration;
		}

		private readonly Settings settings;

		private float desiredSpeed;
		private float speed;

		public KineticMovement(Settings speedSettings)
		{
			settings = speedSettings;
			BroadcastService.Instance.Register(this);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			float deltaSec = gameTime.ElapsedSeconds();

			float addSpeed = speed.CompareTo(desiredSpeed) switch {
				-1 => settings.Acceleration * deltaSec,
				1 => -settings.Deceleration * deltaSec,
				_ => 0f
			};

			var (sin, cos) = MathF.SinCos(gameObject.Rotation);
			speed = Math.Clamp(speed + addSpeed, 0, settings.MaxSpeed);
			gameObject.Position += new Vector2(cos, sin) * speed * deltaSec;
		}

		public void Notify(IBroadcastMessage message, GameTime gameTime)
		{
			switch (message.Tag) {
				case "start_accelerate": desiredSpeed = settings.MaxSpeed; break;
				case "stop_accelerate": desiredSpeed = 0; break;
			}
		}

		protected override void PerformDispose()
		{
			BroadcastService.Instance.Unregister(this);
		}
	}
}
