﻿using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class SuicideOnCollision : IComponent
	{
		private readonly HashSet<int> sensitiveTo;

		public SuicideOnCollision(params int[] sensitiveToGroups)
		{
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
						collider.Owner.TryGetComponent(out DamageProvider provider)
					) {
						healthProvider.Suicide();
						return;
					}
				}
			}
		}
	}
}
