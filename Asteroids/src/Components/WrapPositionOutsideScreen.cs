using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class WrapPositionOutsideScreen : IComponent
	{
		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			var position = gameObject.Position;
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

			gameObject.Position = position;
		}
	}
}
