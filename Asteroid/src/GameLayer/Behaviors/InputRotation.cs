using Asteroids.Core;
using Asteroids.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.GameLayer.Behaviors
{
	public class InputRotation : IBehavior
	{
		private readonly IKeyStateProvider keys;
		private readonly float rps;

		public InputRotation(IKeyStateProvider keyStateProvider, float radianPerSecond)
		{
			keys = keyStateProvider;
			rps = radianPerSecond;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			bool isLeftPressed = keys.IsPressed(Keys.Left);
			bool isRightPressed = keys.IsPressed(Keys.Right);

			if (isLeftPressed == isRightPressed) {
				return;
			}

			float radians = (isRightPressed ? 1 : -1) * rps * gameTime.ElapsedSeconds();
			gameObject.Rotation = MathHelper.WrapAngle(gameObject.Rotation + radians);
		}
	}
}
