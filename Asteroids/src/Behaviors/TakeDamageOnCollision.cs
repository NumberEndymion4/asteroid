using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class TakeDamageOnCollision : Behavior, IDamageAcceptor
	{
		private HealthProvider health;
		private int damageCount;

		public ISet<int> SensitiveTo { get; }

		public TakeDamageOnCollision(GameObject owner, IEnumerable<int> sensitiveToGroups)
			: base(owner)
		{
			SensitiveTo = new HashSet<int>(sensitiveToGroups);
			CollisionService.Instance.Collision += OnCollision;
		}

		public override void Update(GameTime gameTime)
		{
			health ??= Owner.GetBehavior<HealthProvider>();
		}

		private void OnCollision(ICollider lhs, ICollider rhs)
		{
			if (
				health != null &&
				TryGetOpponent(lhs, rhs, out var opponent) &&
				opponent.GetBehavior<IDamageProvider>() is { } provider &&
				SensitiveTo.Contains(provider.DamageGroup)
			) {
				damageCount += provider.Damage;
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
