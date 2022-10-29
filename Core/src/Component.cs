using Microsoft.Xna.Framework;

namespace Core
{
	public abstract class Component : IComponent
	{
		public IGameObject Owner { get; }

		public Component(IGameObject owner)
		{
			Owner = owner;
		}

		public abstract void Update(GameTime gameTime);
	}
}
