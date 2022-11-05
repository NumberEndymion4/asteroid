using Microsoft.Xna.Framework;

namespace Core
{
	public class Config
	{
		public static Config Instance { get; } = new Config();

		public readonly int WindowWidth = 1024;
		public readonly int WindowHeight = 768;

		public readonly Color BackgroundColor = new Color(64, 64, 64);

		public readonly int AsteroidCount = 100;
		public readonly int AsteroidGroup = 1;
		public readonly float AsteroidRadius = 65 / 2f;
		public readonly float AsteroidMinSpeed = 25f;
		public readonly float AsteroidMaxSpeed = 50f;

		public readonly float SpaceshipRadius = 55 / 2f;
		public readonly int BulletGroup = 2;
		public readonly int LaserGroup = 3;

		public Rectangle ScreenRect { get; }

		private Config()
		{
			ScreenRect = new Rectangle(0, 0, WindowWidth, WindowHeight);
		}
	}
}
