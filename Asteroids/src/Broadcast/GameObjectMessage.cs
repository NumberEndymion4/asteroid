using Core;

namespace Asteroids.Broadcast
{
	internal class GameObjectMessage : IBroadcastMessage
	{
		public string Tag { get; }
		public IGameObject Sender { get; }

		public GameObjectMessage(string tag, IGameObject sender)
		{
			Tag = tag;
			Sender = sender;
		}
	}
}
