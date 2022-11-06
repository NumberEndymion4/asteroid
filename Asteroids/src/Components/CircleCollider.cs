using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class CircleCollider : Disposable, ICollider
	{
		private readonly float radius;

		public int Group { get; }
		public BoundingCircle Bounds => new BoundingCircle(Owner.Position, radius * Owner.Scale);
		public IGameObject Owner { get; private set; }

		public CircleCollider(IGameObject owner, int group, float colliderRadius)
		{
			radius = colliderRadius;
			Owner = owner;
			Group = group;
			CollisionService.Instance.Register(this);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
		}

		protected override void PerformDispose()
		{
			CollisionService.Instance.Unregister(this);
			Owner = null;
		}
	}
}
