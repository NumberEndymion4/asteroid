using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class DieOutsideScreen : Component
	{
		private static readonly Rectangle screen = new Rectangle(
			0, 0, Config.Instance.WindowWidth, Config.Instance.WindowHeight
		);

		public DieOutsideScreen(IGameObject owner) : base(owner)
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (screen.Contains(Owner.Position)) {
				return;
			}

			if (Owner.TryGetComponent(out HealthProvider health)) {
				health.Suicide();
			}
		}
	}
}
