using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class CircleCollider : Behavior, ICollider
	{
		private readonly float sourceRadius;

		private Vector2 center;
		private float scale;

		public BoundingCircle Bounds => new BoundingCircle(center, sourceRadius * scale);

		public CircleCollider(IGameObject owner, float radius) : base(owner)
		{
			sourceRadius = radius;
		}

		public override void Update(GameTime gameTime)
		{
			center = Owner.Position;
			scale = Owner.Scale;
		}

		public void CollisionWith(ICollider other)
		{
		}
	}
}
