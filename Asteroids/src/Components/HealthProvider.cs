using System;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class HealthProvider : Component, IDataProvider<LifeCycleState>
	{
		private readonly int healthPoints;

		private int acceptedDamage;

		public int Health => Math.Max(0, healthPoints - acceptedDamage);

		LifeCycleState IDataProvider<LifeCycleState>.Data =>
			Health > 0 ? LifeCycleState.Alive : LifeCycleState.Dead;

		public HealthProvider(IGameObject owner, int health) : base(owner)
		{
			healthPoints = health;
		}

		public void Hit(int damage)
		{
			if (damage > 0 && Health > 0) {
				acceptedDamage += damage;
			}
		}

		public void Suicide()
		{
			acceptedDamage = healthPoints;
		}

		public override void Update(GameTime gameTime)
		{
		}
	}
}
