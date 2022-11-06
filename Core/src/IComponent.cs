using Microsoft.Xna.Framework;

namespace Core
{
	public interface IComponent
	{
		void Update(IGameObject gameObject, GameTime gameTime);
	}
}
