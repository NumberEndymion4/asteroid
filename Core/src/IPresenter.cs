using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
	public interface IPresenter
	{
		void Render(SpriteBatch spriteBatch, GameTime gameTime);
	}
}
