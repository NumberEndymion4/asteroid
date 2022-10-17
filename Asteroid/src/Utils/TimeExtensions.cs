using Microsoft.Xna.Framework;

namespace Asteroids.Utils
{
	public static class TimeExtensions
	{
		public static float ElapsedSeconds(this GameTime gameTime)
		{
			return (float) gameTime.ElapsedGameTime.TotalSeconds;
		}
	}
}
