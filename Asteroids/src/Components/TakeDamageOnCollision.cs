using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class TakeDamageOnCollision : Disposable, IComponent, ITrigger
	{
		private readonly HashSet<IDamageProvider> damagedBy;
		private readonly HashSet<int> sensitiveTo;
		private readonly GameObject owner;

		public TakeDamageOnCollision(GameObject colliderOwner, params int[] sensitiveToGroups)
		{
			owner = colliderOwner;
			damagedBy = new HashSet<IDamageProvider>();
			sensitiveTo = new HashSet<int>(sensitiveToGroups);
			CollisionService.Instance.CollisionEnter += OnCollision;
		}

		public event Action Triggered;

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
		}

		protected override void PerformDispose()
		{
			CollisionService.Instance.CollisionEnter -= OnCollision;
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
				owner.TryGetComponent(out HealthProvider health)
			) {
				if (damagedBy.Add(provider)) {
					health.Hit(provider.Damage);
					Triggered?.Invoke();
				}
			}
		}

		private bool TryGetOpponent(
			ICollider collider1, ICollider collider2, out IGameObject opponent
		) {
			if (Equals(owner, collider1?.Owner)) {
				opponent = collider2?.Owner;
			} else if (Equals(owner, collider2?.Owner)) {
				opponent = collider1?.Owner;
			} else {
				opponent = null;
			}
			return opponent != null;
		}
	}
}
