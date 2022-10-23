using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.AppLayer.Presenters
{
	public class GameObjectPresenter : IPresenter
	{
		private readonly IGameObject gameObject;
		private readonly Texture2D texture;
		private readonly Rectangle region;

		public GameObjectPresenter(
			IGameObject renderObject, Texture2D renderTexture, Rectangle textureRegion
		) {
			gameObject = renderObject;
			texture = renderTexture;
			region = textureRegion;
		}

		public void Render(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				texture,
				gameObject.Position,
				region,
				Color.White,
				gameObject.Rotation,
				region.Size.ToVector2() / 2f,
				gameObject.Scale,
				SpriteEffects.None,
				0.0f
			);
		}
	}
}
