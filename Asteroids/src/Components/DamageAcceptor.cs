using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class DamageAcceptor : IComponent, ITrigger
	{
		private readonly HashSet<object> damagedBy;
		private readonly HashSet<int> sensitiveTo;

		public event Action Triggered;

		public DamageAcceptor(params int[] sensitiveToGroups)
		{
			damagedBy = new HashSet<object>();
			sensitiveTo = new HashSet<int>(sensitiveToGroups);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (!gameObject.TryGetComponent(out HealthProvider healthProvider)) {
				return;
			}

			foreach (var ownCollider in gameObject.EnumerateComponents<ICollider>()) {
				if (!CollisionService.Instance.TryGetCollision(ownCollider, out var colliders)) {
					continue;
				}

				foreach (var collider in colliders) {
					if (
						sensitiveTo.Contains(collider.Group) &&
						collider.Owner.TryGetComponent(out DamageProvider provider) &&
						damagedBy.Add(provider)
					) {
						healthProvider.Hit(provider.Damage);
						Triggered?.Invoke();
						break;
					}
				}
			}
		}
	}
}
