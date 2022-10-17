using Asteroids.Core;
using Asteroids.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.GameLayer.Behaviors
{
	public class InputRotation : IBehavior
	{
		private readonly IGameObject owner;
		private readonly IKeyStateProvider keys;
		private readonly float rps;

		public InputRotation(
			IGameObject gameObject, IKeyStateProvider keyStateProvider, float radianPerSecond
		)
		{
			owner = gameObject;
			keys = keyStateProvider;
			rps = radianPerSecond;
		}

		public void Update(GameTime gameTime)
		{
			bool isLeftPressed = keys.IsPressed(Keys.Left);
			bool isRightPressed = keys.IsPressed(Keys.Right);

			if (isLeftPressed == isRightPressed) {
				return;
			}

			float radians = (isRightPressed ? 1 : -1) * rps * gameTime.ElapsedSeconds();
			owner.Rotation = MathHelper.WrapAngle(owner.Rotation + radians);
		}
	}
}
