using Asteroids.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.AppLayer
{
	public class TextureRegionPresenter : IPresenter
	{
		private readonly Texture2D texture;
		private readonly Rectangle region;

		public TextureRegionPresenter(Texture2D renderTexture, Rectangle textureRegion)
		{
			texture = renderTexture;
			region = textureRegion;
		}

		public void Render(SpriteBatch spriteBatch, IGameObject gameObject)
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
