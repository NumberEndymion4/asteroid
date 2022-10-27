using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class HealthProvider : Behavior, IDataProvider<int>
	{
		public int Data { get; private set; }

		public HealthProvider(IGameObject owner, int health) : base(owner)
		{
			Data = health;
		}

		public override void Update(GameTime gameTime)
		{
		}
	}
}
