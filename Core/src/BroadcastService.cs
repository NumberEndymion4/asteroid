using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
	public partial class BroadcastService
	{
		private readonly HashSet<IBroadcastListener> listeners;
		private readonly List<IBroadcastListener> bucket;
		private readonly Queue<IBroadcastMessage> messages;

		public static BroadcastService Instance { get; } = new BroadcastService();

		private BroadcastService()
		{
			listeners = new HashSet<IBroadcastListener>();
			bucket = new List<IBroadcastListener>();
			messages = new Queue<IBroadcastMessage>();
		}

		public void Register(IBroadcastListener listener)
		{
			listeners.Add(listener);
		}

		public void Unregister(IBroadcastListener listener)
		{
			listeners.Remove(listener);
		}

		public void Schedule(IBroadcastMessage message)
		{
			messages.Enqueue(message);
		}

		public void Update(GameTime gameTime)
		{
			bucket.AddRange(listeners);

			while (messages.TryDequeue(out var message)) {
				for (int i = 0; i < bucket.Count; ++i) {
					bucket[i].Notify(message, gameTime);
				}
			}

			bucket.Clear();
		}
	}
}
