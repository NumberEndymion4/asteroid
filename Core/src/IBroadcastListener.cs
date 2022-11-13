using Microsoft.Xna.Framework;

namespace Core
{
	public interface IBroadcastListener
	{
		void Notify(IBroadcastMessage message, GameTime gameTime);
	}
}
