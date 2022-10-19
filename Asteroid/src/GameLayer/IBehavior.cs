using Asteroids.Core;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer
{
	public interface IBehavior
	{
		void Update(IGameObject gameObject, GameTime gameTime);
	}
}
