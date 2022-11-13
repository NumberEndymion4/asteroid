using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class GameObjectPresenter : IPresenter
	{
		private readonly WeakReference<IGameObject> gameObjectRef;
		private readonly WeakReference<IRenderRegion> regionProviderRef;

		bool IPresenter.IsTargetLost => !gameObjectRef.TryGetTarget(out _);

		public GameObjectPresenter(IGameObject renderObject, IRenderRegion renderRegion)
		{
			gameObjectRef = new WeakReference<IGameObject>(renderObject);
			regionProviderRef = new WeakReference<IRenderRegion>(renderRegion);
		}

		void IPresenter.Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (
				!regionProviderRef.TryGetTarget(out var regionProvider) ||
				!gameObjectRef.TryGetTarget(out var gameObject) ||
				!regionProvider.IsReady
			) {
				return;
			}

			spriteBatch.Draw(
				regionProvider.Texture,
				gameObject.Position,
				regionProvider.Bounds,
				Color.White,
				gameObject.Rotation,
				regionProvider.Bounds.Size.ToVector2() * regionProvider.Origin,
				regionProvider.Scale * gameObject.Scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}
