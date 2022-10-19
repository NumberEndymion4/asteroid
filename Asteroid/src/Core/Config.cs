using Microsoft.Xna.Framework;

namespace Asteroids.Core
{
	public class Config
	{
		public static Config Instance { get; } = new Config();

		public Point WindowSize = new Point(1024, 768);

		public Color BackgroundColor = new Color(64, 64, 64);

		private Config()
		{
		}
	}
}
