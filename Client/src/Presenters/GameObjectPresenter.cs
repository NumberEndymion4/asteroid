using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class GameObjectPresenter : IPresenter
	{
		private readonly IGameObject gameObject;
		private readonly IRenderRegion regionProvider;

		public GameObjectPresenter(IGameObject renderObject, IRenderRegion renderRegion)
		{
			gameObject = renderObject;
			regionProvider = renderRegion;
		}

		void IPresenter.Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (!regionProvider.IsReady) {
				return;
			}

			spriteBatch.Draw(
				regionProvider.Texture,
				gameObject.Position,
				regionProvider.Region,
				Color.White,
				gameObject.Rotation,
				regionProvider.Region.Size.ToVector2() / 2f,
				gameObject.Scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}
