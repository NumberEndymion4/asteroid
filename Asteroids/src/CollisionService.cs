using System.Collections.Generic;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	internal delegate void CollisionHandler(ICollider lhs, ICollider rhs);

	internal class CollisionService
	{
		private readonly Dictionary<ICollider, HashSet<ICollider>> collisions;
		private readonly HashSet<ICollider> entries;
		private readonly List<ICollider> bucket;

		public event CollisionHandler CollisionEnter;
		public event CollisionHandler CollisionExit;

		public static CollisionService Instance { get; } = new CollisionService();

		private CollisionService()
		{
			collisions = new Dictionary<ICollider, HashSet<ICollider>>();
			entries = new HashSet<ICollider>();
			bucket = new List<ICollider>();
		}

		public void Register(ICollider collider)
		{
			entries.Add(collider);
		}

		public void Unregister(ICollider collider)
		{
			foreach (var (_, collection) in collisions) {
				collection.Remove(collider);
			}

			collisions.Remove(collider);
			entries.Remove(collider);
		}

		public void Update(GameTime gameTime)
		{
			bucket.AddRange(entries);

			for (int i = 0, count = bucket.Count; i < count; ++i) {
				var left = bucket[i];

				for (int j = i + 1; j < count; ++j) {
					var right = bucket[j];
					if (left.Group == right.Group) {
						continue;
					}

					if (left.Bounds.IntersectsWith(right.Bounds)) {
						if (
							collisions.TryAppendValueSetOrCreate(left, right) &&
							collisions.TryAppendValueSetOrCreate(right, left)
						) {
							CollisionEnter?.Invoke(left, right);
						}
					} else {
						if (
							collisions.TryRemoveFromValueCollection(left, right) &&
							collisions.TryRemoveFromValueCollection(right, left)
						) {
							CollisionExit?.Invoke(left, right);
						}
					}
				}
			}

			bucket.Clear();
		}
	}
}
