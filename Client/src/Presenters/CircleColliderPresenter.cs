using System;
using Core;
using Core.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class CircleColliderPresenter : IPresenter
	{
		private readonly CircleCollider collider;
		private readonly Texture2D texture;

		bool IPresenter.IsTargetLost => collider.Owner == null;

		public CircleColliderPresenter(CircleCollider renderCollider, Texture2D renderTexture)
		{
			collider = renderCollider;
			texture = renderTexture;
		}

		public void Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.Draw(
				texture,
				collider.Center,
				texture.Bounds,
				Color.White,
				0f,
				texture.Bounds.Center.ToVector2(),
				collider.Diameter / Math.Min(texture.Width, texture.Height),
				SpriteEffects.None,
				0.0f
			);
		}
	}
}
