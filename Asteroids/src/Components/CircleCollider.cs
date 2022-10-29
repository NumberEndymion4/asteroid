using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class CircleCollider : Component, ICollider
	{
		private readonly float radius;

		private Vector2 center;
		private float scale;

		public BoundingCircle Bounds => new BoundingCircle(center, radius * scale);

		public CircleCollider(IGameObject owner, float colliderRadius) : base(owner)
		{
			radius = colliderRadius;
			CollisionService.Instance.Register(this);
		}

		public override void Update(GameTime gameTime)
		{
			center = Owner.Position;
			scale = Owner.Scale;
		}
	}
}
