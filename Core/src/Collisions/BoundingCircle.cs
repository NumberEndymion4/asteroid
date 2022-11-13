using Microsoft.Xna.Framework;

namespace Core.Collisions
{
	public readonly struct BoundingCircle
	{
		public readonly Vector2 Center;
		public readonly float Radius;

		public float Diameter => 2 * Radius;

		public BoundingCircle(Vector2 center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		public bool IntersectsWith(in BoundingCircle other)
		{
			float radiusSum = Radius + other.Radius;
			return Vector2.DistanceSquared(Center, other.Center) < radiusSum * radiusSum;
		}
	}
}
