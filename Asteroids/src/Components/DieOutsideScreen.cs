using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class DieOutsideScreen : IComponent
	{
		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (!Config.Instance.ScreenRect.Contains(gameObject.Position)) {
				gameObject.GetComponent<HealthProvider>()?.Suicide();
			}
		}
	}
}
