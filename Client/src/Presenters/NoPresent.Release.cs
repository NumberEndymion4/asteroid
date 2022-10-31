using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class NoPresent : IPresenter
	{
		void IPresenter.Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
		}
	}
}
