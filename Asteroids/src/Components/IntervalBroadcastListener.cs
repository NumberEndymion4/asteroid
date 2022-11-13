using System;
using Asteroids.Broadcast;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class IntervalBroadcastListener
		: Disposable, IDataProvider<TimeSpan>, IBroadcastListener
	{
		private readonly string tag;
		private readonly string startOn;
		private readonly string stopOn;
		private readonly TimeSpan interval;

		private TimeSpan timeToTrigger;
		private bool isTriggered;

		TimeSpan IDataProvider<TimeSpan>.Data => timeToTrigger;

		public IntervalBroadcastListener(
			string broadcastTag,
			string tagStart,
			string tagStop,
			TimeSpan broadcastInterval,
			bool resetInterval
		) {
			tag = broadcastTag;
			startOn = tagStart;
			stopOn = tagStop;
			interval = broadcastInterval;
			timeToTrigger = resetInterval ? interval : TimeSpan.Zero;

			BroadcastService.Instance.Register(this);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (timeToTrigger > gameTime.ElapsedGameTime) {
				timeToTrigger -= gameTime.ElapsedGameTime;
			} else if (isTriggered) {
				timeToTrigger += interval - gameTime.ElapsedGameTime;
				BroadcastService.Instance.Schedule(new GameObjectMessage(tag, gameObject));
			} else {
				timeToTrigger = TimeSpan.Zero;
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
