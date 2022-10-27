using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	public class GameObject : IGameObject
	{
		private readonly List<IBehavior> behaviors;

		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }

		public GameObject()
		{
			behaviors = new List<IBehavior>();
			Scale = 1f;
		}

		public void Update(GameTime gameTime)
		{
			foreach (var behavior in behaviors) {
				behavior.Update(gameTime);
			}
		}

		public void AddBehavior(IBehavior behavior)
		{
			behaviors.Add(behavior);
		}

		public TBehavior GetBehavior<TBehavior>() where TBehavior : IBehavior
		{
			return TryGetBehavior(out TBehavior behavior) ? behavior : default;
		}

		public bool TryGetBehavior<TBehavior>(out TBehavior behavior) where TBehavior : IBehavior
		{
			foreach (var item in behaviors) {
				if (item is TBehavior found) {
					behavior = found;
					return true;
				}
			}

			behavior = default;
			return false;
		}
	}
}
