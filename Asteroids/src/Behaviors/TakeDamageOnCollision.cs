using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class TakeDamageOnCollision : Behavior, IDamageAcceptor
	{
		public int Health { get; private set; }
		public ISet<int> SensitiveTo { get; }

		public TakeDamageOnCollision(
			GameObject owner, int health, IEnumerable<int> sensitiveToGroups
		) : base(
			owner
		) {
			SensitiveTo = new HashSet<int>(sensitiveToGroups);
			Health = health;

			CollisionService.Instance.Collision += OnCollision;
		}

		public override void Update(GameTime gameTime)
		{
		}

		private void OnCollision(ICollider lhs, ICollider rhs)
		{
			var pairOwner = Equals(Owner, lhs.Owner)
				? rhs.Owner
				: Equals(Owner, rhs.Owner) ? lhs.Owner : null;

			if (
				pairOwner?.GetBehavior<IDamageProvider>() is { } provider &&
				SensitiveTo.Contains(provider.DamageGroup)
			) {
				Health -= provider.Damage;
			}
		}
	}
}
