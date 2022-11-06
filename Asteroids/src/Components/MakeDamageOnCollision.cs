using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class MakeDamageOnCollision : IComponent, IDamageProvider
	{
		public int DamageGroup { get; }
		public int Damage { get; }

		public MakeDamageOnCollision(int damageGroup, int damage)
		{
			DamageGroup = damageGroup;
			Damage = damage;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
		}
	}
}
