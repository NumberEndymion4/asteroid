using Microsoft.Xna.Framework;

namespace Core.Collisions
{
	public abstract class GameObjectCollider : Disposable, ICollider
	{
		public int Group { get; }
		public IGameObject Owner { get; private set; }

		public GameObjectCollider(IGameObject owner, int group)
		{
			Owner = owner;
			Group = group;
			CollisionService.Instance.Register(this);
		}

		public abstract void Update(IGameObject gameObject, GameTime gameTime);

		protected override void PerformDispose()
		{
			CollisionService.Instance.Unregister(this);
			Owner = null;
		}
	}
}
