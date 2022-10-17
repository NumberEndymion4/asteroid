using Microsoft.Xna.Framework;

namespace Asteroids.Core
{
	public class Config
	{
		public static Config Instance { get; } = new Config();

		public int WindowWidth = 1024;
		public int WindowHeight = 768;

		public Color BackgroundColor = new Color(64, 64, 64);

		private Config()
		{
		}
	}
}
