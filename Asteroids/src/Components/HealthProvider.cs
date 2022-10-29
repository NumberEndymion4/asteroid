using System;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class HealthProvider : Component, IDataProvider<LifeCycleState>
	{
		private readonly int healthPoints;

		private int acceptedDamage;
		private bool isDead;

		public int Health => Math.Max(0, healthPoints - acceptedDamage);

		LifeCycleState IDataProvider<LifeCycleState>.Data =>
			isDead ? LifeCycleState.Dead : LifeCycleState.Alive;

		public HealthProvider(IGameObject owner, int health) : base(owner)
		{
			healthPoints = health;
		}

		public void Hit(int damage)
		{
			if (!isDead && damage > 0) {
				acceptedDamage += damage;
				isDead = Health == 0;
			}
		}

		public override void Update(GameTime gameTime)
		{
		}
	}
}
