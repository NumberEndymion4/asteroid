using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class TakeDamageOnCollision : Component, IDamageAcceptor
	{
		private readonly HashSet<IDamageProvider> damagedBy;
		private readonly HashSet<int> sensitiveTo;

		public TakeDamageOnCollision(GameObject owner, params int[] sensitiveToGroups) : base(owner)
		{
			damagedBy = new HashSet<IDamageProvider>();
			sensitiveTo = new HashSet<int>(sensitiveToGroups);
			CollisionService.Instance.Collision += OnCollision;
		}

		public override void Update(GameTime gameTime)
		{
		}

		protected override void PerformDispose()
		{
			CollisionService.Instance.Collision -= OnCollision;
			sensitiveTo.Clear();
			damagedBy.Clear();
			base.PerformDispose();
		}

		private void OnCollision(ICollider lhs, ICollider rhs)
		{
			if (
				TryGetOpponent(lhs, rhs, out var opponent) &&
				opponent.TryGetComponent(out IDamageProvider provider) &&
				sensitiveTo.Contains(provider.DamageGroup) &&
				Owner.TryGetComponent(out HealthProvider health)
			) {
				if (damagedBy.Add(provider)) {
					health.Hit(provider.Damage);
				}
			}
		}

		private bool TryGetOpponent(
			ICollider collider1, ICollider collider2, out IGameObject opponent
		) {
			if (Equals(Owner, collider1?.Owner)) {
				opponent = collider2?.Owner;
			} else if (Equals(Owner, collider2?.Owner)) {
				opponent = collider1?.Owner;
			} else {
				opponent = null;
			}
			return opponent != null;
		}
	}
}
