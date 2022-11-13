using System.Collections.Generic;
using System.Diagnostics;
using Core.Collisions;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Core
{
	public class CollisionService
	{
		private readonly Dictionary<ICollider, HashSet<ICollider>> collisions;
		private readonly HashSet<ICollider> entries;
		private readonly List<ICollider> bucket;

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
			foreach (var (_, colliders) in collisions) {
				colliders.Remove(collider);
			}

			collisions.Remove(collider);
			entries.Remove(collider);
		}

		public bool TryGetCollision(ICollider left, out IReadOnlySet<ICollider> colliders)
		{
			colliders = collisions.GetValueOrDefault(left);
			return colliders != null;
		}

		public void Update()
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
						bool leftAdded = collisions.TryAppendValueSetOrCreate(left, right);
						bool rightAdded = collisions.TryAppendValueSetOrCreate(right, left);
						Debug.Assert(leftAdded == rightAdded);
					} else {
						bool leftRemoved = collisions.TryRemoveFromValueCollection(left, right);
						bool rightRemoved = collisions.TryRemoveFromValueCollection(right, left);
						Debug.Assert(leftRemoved == rightRemoved);
					}
				}
			}

			bucket.Clear();
		}
	}
}
