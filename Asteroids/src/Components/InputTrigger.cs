using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Components
{
	internal class InputTrigger : Component, ITrigger
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

		public InputTrigger(IGameObject owner, Settings triggerSettings) : base(owner)
		{
			settings = triggerSettings;
		}

		public override void Update(GameTime gameTime)
		{
			if (!settings.KeyStateProvider.IsPressed(settings.TriggerOn)) {
				return;
			}

			if (gameTime.TotalGameTime - lastTriggerTime > settings.Interval) {
				lastTriggerTime = gameTime.TotalGameTime;
				Triggered?.Invoke();
			}
		}
	}
}
