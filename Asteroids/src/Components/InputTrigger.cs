using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Components
{
	internal class InputTrigger : Disposable, IComponent, ITrigger
	{
		public struct Settings
		{
			public IKeyStateProvider KeyStateProvider;
			public TimeSpan Interval;
			public Keys TriggerOn;
		}

		public event Action Triggered;

		private readonly Settings settings;
		private TimeSpan lastTriggerTime;

		public InputTrigger(Settings triggerSettings)
		{
			settings = triggerSettings;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (!settings.KeyStateProvider.IsPressed(settings.TriggerOn)) {
				return;
			}

			if (gameTime.TotalGameTime - lastTriggerTime > settings.Interval) {
				lastTriggerTime = gameTime.TotalGameTime;
				Triggered?.Invoke();
			}
		}

		protected override void PerformDispose()
		{
			Triggered = null;
		}
	}
}
