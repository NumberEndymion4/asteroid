using System.Collections.Generic;
using Client.Components;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class GameObjectPresenter : IPresenter
	{
		private readonly Dictionary<string, SpriteAnimation> animations;
		private readonly IGameObject gameObject;
		private readonly AnimationIdProvider animationProvider;

		private string animationName;

		public GameObjectPresenter(
			IGameObject renderObject, AnimationIdProvider animationIdProvider
		) {
			animations = new Dictionary<string, SpriteAnimation>();
			gameObject = renderObject;
			animationProvider = animationIdProvider;
		}

		public void AddSpriteAnimation(string animationId, SpriteAnimation animation)
		{
			animations.TryAdd(animationId, animation);
		}

		void IPresenter.Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
			var playAnimationName = animationProvider.CurrentAnimationId;
			if (!animations.TryGetValue(playAnimationName, out var playAnimation)) {
				return;
			}

			if (animationName != playAnimationName) {
				animationName = playAnimationName;
				playAnimation.Start(gameTime);
			}

			playAnimation.Continue(gameTime, out var region);

			spriteBatch.Draw(
				playAnimation.Texture,
				gameObject.Position,
				region,
				Color.White,
				gameObject.Rotation,
				region.Size.ToVector2() / 2f,
				gameObject.Scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}
