using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer.Behaviors
{
	public class CircleCollider : IBehavior, ICollider
	{
		private readonly float sourceRadius;

		private Vector2 center;
		private float scale;

		public BoundingCircle Bounds => new BoundingCircle(center, sourceRadius * scale);

		public CircleCollider(float radius)
		{
			sourceRadius = radius;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			center = gameObject.Position;
			scale = gameObject.Scale;
		}

		public void CollisionWith(ICollider other)
		{
		}
	}
}
