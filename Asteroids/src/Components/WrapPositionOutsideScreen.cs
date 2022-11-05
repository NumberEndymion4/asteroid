using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class WrapPositionOutsideScreen : Component
	{
		public WrapPositionOutsideScreen(IGameObject owner) : base(owner)
		{
		}

		public override void Update(GameTime gameTime)
		{
			var position = Owner.Position;
			var screenRect = Config.Instance.ScreenRect;

			if (position.X < screenRect.Left) {
				position.X += screenRect.Width;
			} else if (position.X > screenRect.Right) {
				position.X -= screenRect.Width;
			}

			if (position.Y < screenRect.Top) {
				position.Y += screenRect.Height;
			} else if (position.Y > screenRect.Bottom) {
				position.Y -= screenRect.Height;
			}

			Owner.Position = position;
		}
	}
}
