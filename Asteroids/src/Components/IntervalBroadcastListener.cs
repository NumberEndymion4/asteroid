using System;
using Asteroids.Broadcast;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class IntervalBroadcastListener : Disposable, IComponent, IBroadcastListener
	{
		private readonly string tag;
		private readonly string startOn;
		private readonly string stopOn;
		private readonly TimeSpan interval;

		private TimeSpan? lastTriggerTime;
		private bool isTriggered;

		public IntervalBroadcastListener(
			string broadcastTag, string tagStart, string tagStop, TimeSpan broadcastInterval
		) {
			tag = broadcastTag;
			startOn = tagStart;
			stopOn = tagStop;
			interval = broadcastInterval;
			BroadcastService.Instance.Register(this);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (!isTriggered || gameTime.TotalGameTime - lastTriggerTime < interval) {
				return;
			}

			BroadcastService.Instance.Schedule(new GameObjectMessage(tag, gameObject));
			lastTriggerTime = gameTime.TotalGameTime;
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
