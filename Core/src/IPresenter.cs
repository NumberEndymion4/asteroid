using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
	public interface IPresenter
	{
		bool IsTargetLost { get; }

		void Render(SpriteBatch spriteBatch, GameTime gameTime);
	}
}
