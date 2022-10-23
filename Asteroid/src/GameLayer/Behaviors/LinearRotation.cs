﻿using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer.Behaviors
{
	public class LinearRotation : IBehavior
	{
		private readonly float rps;

		public LinearRotation(float radianPerSecond)
		{
			rps = radianPerSecond;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			gameObject.Rotation += rps * gameTime.ElapsedSeconds();
		}
	}
}
