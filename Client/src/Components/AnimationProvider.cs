using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Components
{
	internal class AnimationProvider : Component, IRenderRegion
	{
		private readonly Dictionary<string, SpriteAnimation> animations;

		private SpriteAnimation playAnimation;
		private string playAnimationName;
		private Rectangle playRegion;

		public Texture2D Texture => playAnimation?.Texture;
		public Rectangle Region => playRegion;
		public bool IsReady => playAnimation != null;

		public AnimationProvider(IGameObject owner) : base(owner)
		{
			animations = new Dictionary<string, SpriteAnimation>();
		}

		public void AddSpriteAnimation(string animationId, SpriteAnimation animation)
		{
			animations.TryAdd(animationId, animation);
		}

		public override void Update(GameTime gameTime)
		{
			var lifeCycleProvider = Owner.GetComponent<IDataProvider<LifeCycleState>>();
			var animationName = lifeCycleProvider?.Data.ToString() ?? string.Empty;

			if (!animations.TryGetValue(animationName, out playAnimation)) {
				return;
			}

			if (playAnimationName != animationName) {
				playAnimationName = animationName;
				playAnimation.Start(gameTime);
			}
			playAnimation.Continue(gameTime, out playRegion);
		}
	}
}
