using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Components
{
	internal class AnimationProvider : Disposable, IComponent, IRenderRegion, ICompletable
	{
		private readonly Dictionary<string, SpriteAnimation> animations;

		private SpriteAnimation playAnimation;
		private string playAnimationName;
		private SpriteAnimation.Region playRegion;

		public Texture2D Texture => playAnimation?.Texture;
		public Rectangle Bounds => playRegion?.Bounds ?? Rectangle.Empty;
		public Vector2 Origin => playRegion?.Origin ?? new Vector2(0.5f);
		public Vector2 Scale => playRegion?.Scale ?? Vector2.One;

		public bool IsReady => playAnimation != null;
		public bool IsComplete => playAnimation?.IsPlaying != true;

		public AnimationProvider()
		{
			animations = new Dictionary<string, SpriteAnimation>();
		}

		public void AddSpriteAnimation(string animationId, SpriteAnimation animation)
		{
			animations.TryAdd(animationId, animation);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			var lifeCycleProvider = gameObject.GetComponent<IDataProvider<LifeCycleState>>();
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

		protected override void PerformDispose()
		{
			animations.Clear();
			playAnimation = null;
			playAnimationName = null;
		}
	}
}
