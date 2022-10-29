using Microsoft.Xna.Framework;

namespace Core
{
	public interface IComponent
	{
		IGameObject Owner { get; }

		void Update(GameTime gameTime);
	}
}
