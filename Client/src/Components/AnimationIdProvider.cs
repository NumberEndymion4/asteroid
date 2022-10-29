using Core;
using Microsoft.Xna.Framework;

namespace Client.Components
{
	internal class AnimationIdProvider : Component
	{
		public string CurrentAnimationId { get; private set; }

		public AnimationIdProvider(IGameObject owner) : base(owner)
		{
			CurrentAnimationId = string.Empty;
		}

		public override void Update(GameTime gameTime)
		{
			var lifeCycleProvider = Owner.GetComponent<IDataProvider<LifeCycleState>>();
			CurrentAnimationId = lifeCycleProvider?.Data.ToString() ?? string.Empty;
		}
	}
}
