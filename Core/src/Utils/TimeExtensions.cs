using Microsoft.Xna.Framework;

namespace Core.Utils
{
	public static class TimeExtensions
	{
		public static float ElapsedSeconds(this GameTime gameTime)
		{
			return (float) gameTime.ElapsedGameTime.TotalSeconds;
		}
	}
}
