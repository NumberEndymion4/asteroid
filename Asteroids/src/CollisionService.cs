using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	internal delegate void CollisionHandler(ICollider lhs, ICollider rhs);

	internal class CollisionService
	{
		private readonly HashSet<ICollider> entries;
		private readonly List<ICollider> bucket;

		public event CollisionHandler Collision;

		public static CollisionService Instance { get; } = new CollisionService();

		private CollisionService()
		{
			entries = new HashSet<ICollider>();
			bucket = new List<ICollider>();
		}

		public void Register(ICollider collider)
		{
			entries.Add(collider);
		}

		public void Unregister(ICollider collider)
		{
			entries.Remove(collider);
		}

		public void Update(GameTime gameTime)
		{
			bucket.AddRange(entries);

			for (int i = 0, count = bucket.Count; i < count; ++i) {
				for (int j = i + 1; j < count; ++j) {
					if (bucket[i].Bounds.IntersectsWith(bucket[j].Bounds)) {
						Collision?.Invoke(bucket[i], bucket[j]);
					}
				}
			}

			bucket.Clear();
		}
	}
}