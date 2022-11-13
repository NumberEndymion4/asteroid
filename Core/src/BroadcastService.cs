using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
	public partial class BroadcastService
	{
		private readonly HashSet<IBroadcastListener> listeners;
		private readonly Queue<IBroadcastMessage> messages;

		public static BroadcastService Instance { get; } = new BroadcastService();

		private BroadcastService()
		{
			listeners = new HashSet<IBroadcastListener>();
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

		public void Notify(GameTime gameTime)
		{
			while (messages.TryDequeue(out var message)) {
				foreach (var listener in listeners) {
					listener.Notify(message, gameTime);
				}
			}
		}
	}
}
