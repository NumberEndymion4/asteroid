using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class HealthProvider : Component, IDataProvider<int>
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
