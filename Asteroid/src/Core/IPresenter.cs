using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Core
{
	public interface IPresenter
	{
		void Render(SpriteBatch spriteBatch, IGameObject gameObject);
	}
}
