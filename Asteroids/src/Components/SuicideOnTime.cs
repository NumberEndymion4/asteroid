using System;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class SuicideOnTime : IComponent
	{
		private readonly TimeSpan suicideIn;
		private TimeSpan? startTime;
		private bool wasSuicide;

		public SuicideOnTime(TimeSpan lifeTime)
		{
			suicideIn = lifeTime;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (wasSuicide) {
				return;
			}

			if (!startTime.HasValue) {
				startTime = gameTime.TotalGameTime;
			} else if (gameTime.TotalGameTime - startTime > suicideIn) {
				gameObject.GetComponent<HealthProvider>()?.Suicide();
				wasSuicide = true;
			}
		}
	}
}
