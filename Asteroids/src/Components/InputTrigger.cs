using System;
using Asteroids.Broadcast;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class InputTrigger : Disposable, IComponent, IBroadcastListener
	{
		private readonly string tag;
		private readonly string startOn;
		private readonly string stopOn;
		private readonly TimeSpan interval;

		private TimeSpan? lastTriggerTime;
		private bool isTriggered;

		public InputTrigger(string spawnTag, string tagStart)
			: this(spawnTag, tagStart, null, TimeSpan.Zero)
		{
		}

		public InputTrigger(
			string spawnTag, string tagStart, string tagStop, TimeSpan spawnInterval
		) {
			tag = spawnTag;
			startOn = tagStart;
			stopOn = tagStop;
			interval = spawnInterval;
			BroadcastService.Instance.Register(this);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (!isTriggered || gameTime.TotalGameTime - lastTriggerTime < interval) {
				return;
			}

			BroadcastService.Instance.Schedule(new GameObjectMessage(tag, gameObject));
			lastTriggerTime = gameTime.TotalGameTime;

			if (stopOn == null) {
				isTriggered = false;
			}
		}

		protected override void PerformDispose()
		{
			BroadcastService.Instance.Unregister(this);
		}

		void IBroadcastListener.Notify(IBroadcastMessage message, GameTime gameTime)
		{
			if (message.Tag == startOn) {
				isTriggered = true;
			} else if (message.Tag == stopOn) {
				isTriggered = false;
			}
		}
	}
}
