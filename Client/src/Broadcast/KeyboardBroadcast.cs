using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client.Broadcast
{
	internal class KeyboardBroadcast
	{
		private class KeyState
		{
			private readonly string pressedTag;
			private readonly string releasedTag;

			public bool IsPressed { get; set; }
			public string Tag => IsPressed ? pressedTag : releasedTag;

			public KeyState(string pressed, string released)
			{
				pressedTag = pressed;
				releasedTag = released;
			}
		}

		private readonly Dictionary<Keys, KeyState> keyTags;

		public KeyboardBroadcast()
		{
			keyTags = new Dictionary<Keys, KeyState>();
		}

		public void RegisterTags(Keys key, string pressTag, string releaseTag)
		{
			keyTags.TryAdd(key, new KeyState(pressTag, releaseTag));
		}

		public void Update(GameTime gameTime)
		{
			var keyboard = Keyboard.GetState();
			foreach (var (key, state) in keyTags) {
				if (keyboard.IsKeyDown(key) != state.IsPressed) {
					state.IsPressed = !state.IsPressed;
					BroadcastService.Instance.Schedule(new KeyboardMessage(state.Tag));
				}
			}
		}
	}
}
