using Microsoft.Xna.Framework;

namespace Core
{
	public abstract class Behavior : IBehavior
	{
		public IGameObject Owner { get; }

		protected Behavior(IGameObject owner)
		{
			Owner = owner;
		}

		public abstract void Update(GameTime gameTime);
	}
}
