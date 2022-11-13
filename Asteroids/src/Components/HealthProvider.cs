using System;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class HealthProvider : IDataProvider<LifeCycleState>
	{
		private readonly int healthPoints;

		private bool wasSuicide;
		private int acceptedDamage;

		public int Health { get; private set; }

		LifeCycleState IDataProvider<LifeCycleState>.Data =>
			(!wasSuicide && Health > 0) ? LifeCycleState.Alive : LifeCycleState.Dead;

		public HealthProvider(int health)
		{
			healthPoints = health;
			Health = health;
		}

		public void Hit(int damage)
		{
			if (damage > 0) {
				acceptedDamage = Math.Min(healthPoints, acceptedDamage + damage);
			}
		}

		public void Suicide()
		{
			wasSuicide = true;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			Health = Math.Max(0, healthPoints - acceptedDamage);
		}
	}
}
