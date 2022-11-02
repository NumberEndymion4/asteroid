using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class NoPresent : IPresenter
	{
		public bool IsTargetLost => false;

		void IPresenter.Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
		}
	}
}
