using System;
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

					if (HasIntersection(left, right)) {
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

		private static bool HasIntersection(ICollider left, ICollider right)
		{
			return left switch {
				CircleCollider leftCircle => right switch {
					CircleCollider rightCircle => HasIntersection(leftCircle, rightCircle),
					LineCollider rightLine => HasIntersection(leftCircle, rightLine),
					_ => throw UnknownCollision(left, right)
				},
				LineCollider leftLine => right switch {
					CircleCollider rightCircle => HasIntersection(rightCircle, leftLine),
					LineCollider rightLine => HasIntersection(leftLine, rightLine),
					_ => throw UnknownCollision(left, right)
				},
				_ => throw UnknownCollision(left, right)
			};

			static Exception UnknownCollision(ICollider left, ICollider right)
			{
				return new InvalidOperationException(
					$"Unknown collision between {left.GetType()} and {right.GetType()}."
				);
			}
		}

		private static bool HasIntersection(CircleCollider left, CircleCollider right)
		{
			float radiusSum = left.Radius + right.Radius;
			float distanceSquared = Vector2.DistanceSquared(left.Center, right.Center);
			return distanceSquared < radiusSum * radiusSum;
		}

		private static bool HasIntersection(CircleCollider left, LineCollider right)
		{
			float radiusSquared = left.Radius * left.Radius;
			if (
				Vector2.DistanceSquared(left.Center, right.Start) < radiusSquared ||
				Vector2.DistanceSquared(left.Center, right.End) < radiusSquared
			) {
				return true;
			}

			var right0 = right.End - right.Start;
			var left0 = left.Center - right.Start;
			float dot = Vector2.Dot(left0, right0);
			var closest = right.Start + (dot / right0.LengthSquared()) * right0;

			if (Vector2.DistanceSquared(closest, left.Center) > radiusSquared) {
				return false;
			}

			const float Epsilon = 1f;
			float d0 = Vector2.Distance(right.Start, right.End);
			float d1 = Vector2.Distance(closest, right.Start);
			float d2 = Vector2.Distance(closest, right.End);
			return d1 + d2 > d0 - Epsilon && d1 + d2 < d0 + Epsilon;
		}

		private static bool HasIntersection(LineCollider left, LineCollider right)
		{
			throw new NotImplementedException();
		}
	}
}
