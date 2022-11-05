using System;
using Microsoft.Xna.Framework;

namespace Core
{
	public class ObstacleSettings
	{
		public float ScaleMin;
		public float ScaleMax;

		public float AngleVelocityMin;
		public float AngleVelocityMax;
	}

	public class Config
	{
		public static Config Instance { get; } = new Config();

		public readonly int WindowWidth = 1024;
		public readonly int WindowHeight = 768;

		public readonly Color BackgroundColor = new Color(64, 64, 64);

		public readonly int AsteroidCount = 20;
		public readonly int AsteroidGroup = 1;
		public readonly float AsteroidRadius = 65 / 2f;
		public readonly float AsteroidMinSpeed = 25f;
		public readonly float AsteroidMaxSpeed = 50f;
		public readonly int AsteroidSpawnCount = 4;

		public readonly ObstacleSettings BigAsteroid = new ObstacleSettings {
			ScaleMin = 0.6f,
			ScaleMax = 1f,
			AngleVelocityMin = MathF.PI / 24,
			AngleVelocityMax = MathF.PI / 16,
		};

		public readonly ObstacleSettings SmallAsteroid = new ObstacleSettings {
			ScaleMin = 0.2f,
			ScaleMax = 0.4f,
			AngleVelocityMin = MathF.PI / 16,
			AngleVelocityMax = MathF.PI / 8,
		};

		public readonly int SpaceshipGroup = 0;
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
