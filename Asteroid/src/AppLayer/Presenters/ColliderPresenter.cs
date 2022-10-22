using System;
using Asteroids.Core;
using Asteroids.GameLayer.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.AppLayer.Presenters
{
	public class ColliderPresenter : IPresenter
	{
		private readonly CircleCollider collider;
		private readonly Texture2D texture;

		public ColliderPresenter(CircleCollider renderCollider, Texture2D renderTexture)
		{
			collider = renderCollider;
			texture = renderTexture;
		}

		public void Render(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				texture,
				collider.Bounds.Center,
				texture.Bounds,
				Color.White,
				0f,
				texture.Bounds.Center.ToVector2(),
				collider.Bounds.Diameter / Math.Min(texture.Width, texture.Height),
				SpriteEffects.None,
				0.0f
			);
		}
	}
}
