using Core;

namespace Client.Broadcast
{
	internal class KeyboardMessage : IBroadcastMessage
	{
		public string Tag { get; }

		public KeyboardMessage(string tag)
		{
			Tag = tag;
		}
	}
}
