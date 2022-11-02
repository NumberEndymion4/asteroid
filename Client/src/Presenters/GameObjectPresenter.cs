using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class GameObjectPresenter : IPresenter
	{
		private readonly WeakReference<IGameObject> gameObjectRef;
		private readonly IRenderRegion regionProvider;

		bool IPresenter.IsTargetLost => !gameObjectRef.TryGetTarget(out _);

		public GameObjectPresenter(IGameObject renderObject, IRenderRegion renderRegion)
		{
			gameObjectRef = new WeakReference<IGameObject>(renderObject);
			regionProvider = renderRegion;
		}

		void IPresenter.Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (!regionProvider.IsReady || !gameObjectRef.TryGetTarget(out var gameObject)) {
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
