using Microsoft.Xna.Framework;

namespace Core.Collisions
{
	public class CircleCollider : GameObjectCollider
	{
		private readonly float radius;

		public Vector2 Center => Owner.Position;
		public float Radius => radius * Owner.Scale;
		public float Diameter => 2 * Radius;

		public CircleCollider(IGameObject owner, int group, float colliderRadius)
			: base(owner, group)
		{
			radius = colliderRadius;
		}

		public override void Update(IGameObject gameObject, GameTime gameTime)
		{
		}
	}
}
