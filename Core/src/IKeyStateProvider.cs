using Microsoft.Xna.Framework.Input;

namespace Core
{
	public interface IKeyStateProvider
	{
		bool IsPressed(Keys keys);
	}
}
