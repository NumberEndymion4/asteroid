using Microsoft.Xna.Framework;

namespace Core
{
	public interface IBehavior
	{
		IGameObject Owner { get; }

		void Update(GameTime gameTime);
	}
}
