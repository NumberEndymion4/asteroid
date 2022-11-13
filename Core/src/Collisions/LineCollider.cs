using Microsoft.Xna.Framework;

namespace Core.Collisions
{
	public class LineCollider : GameObjectCollider
	{
		public Vector2 Start { get; }
		public Vector2 End { get; }

		public LineCollider(IGameObject owner, int group, Vector2 start, Vector2 end)
			: base(owner, group)
		{
			Start = start;
			End = end;
		}

		public override void Update(IGameObject gameObject, GameTime gameTime)
		{
		}
	}
}
