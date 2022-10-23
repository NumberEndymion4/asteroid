using Microsoft.Xna.Framework;

namespace Core
{
	public interface IBehavior
	{
		void Update(IGameObject gameObject, GameTime gameTime);
	}
}
