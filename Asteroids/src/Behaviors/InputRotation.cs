using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Behaviors
{
	internal class InputRotation : Behavior
	{
		private readonly IKeyStateProvider keys;
		private readonly float rps;

		public InputRotation(
			IGameObject owner, IKeyStateProvider keyStateProvider, float radianPerSecond
		) : base(
			owner
		) {
			keys = keyStateProvider;
			rps = radianPerSecond;
		}

		public override void Update(GameTime gameTime)
		{
			bool isLeftPressed = keys.IsPressed(Keys.Left);
			bool isRightPressed = keys.IsPressed(Keys.Right);

			if (isLeftPressed == isRightPressed) {
				return;
			}

			float radians = (isRightPressed ? 1 : -1) * rps * gameTime.ElapsedSeconds();
			Owner.Rotation = MathHelper.WrapAngle(Owner.Rotation + radians);
		}
	}
}
