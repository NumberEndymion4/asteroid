using Core;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	internal abstract class Behavior : IBehavior
	{
		public IGameObject Owner { get; }

		protected Behavior(IGameObject owner)
		{
			Owner = owner;
		}

		public abstract void Update(GameTime gameTime);
	}
}
