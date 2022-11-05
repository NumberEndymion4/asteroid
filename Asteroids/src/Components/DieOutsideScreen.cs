using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class DieOutsideScreen : Component
	{
		public DieOutsideScreen(IGameObject owner) : base(owner)
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (!Config.Instance.ScreenRect.Contains(Owner.Position)) {
				Owner.GetComponent<HealthProvider>()?.Suicide();
			}
		}
	}
}
