using Microsoft.Xna.Framework.Input;

namespace Asteroids.Core
{
	public interface IKeyStateProvider
	{
		bool IsPressed(Keys keys);
	}
}
