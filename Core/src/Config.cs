using Microsoft.Xna.Framework;

namespace Core
{
	public class Config
	{
		public static Config Instance { get; } = new Config();

		public readonly int WindowWidth = 1024;
		public readonly int WindowHeight = 768;

		public readonly Color BackgroundColor = new Color(64, 64, 64);

		public readonly int EnemyCount = 100;

		public readonly int AsteroidGroup = 1;
		public readonly int BulletGroup = 2;
		public readonly int LaserGroup = 3;

		public Rectangle ScreenRect { get; }

		private Config()
		{
			ScreenRect = new Rectangle(0, 0, WindowWidth, WindowHeight);
		}
	}
}
