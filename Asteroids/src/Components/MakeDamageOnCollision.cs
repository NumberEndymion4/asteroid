using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class MakeDamageOnCollision : Component, IDamageProvider
	{
		public int DamageGroup { get; }
		public int Damage { get; }

		public MakeDamageOnCollision(IGameObject owner, int damageGroup, int damage) : base(owner)
		{
			DamageGroup = damageGroup;
			Damage = damage;
		}

		public override void Update(GameTime gameTime)
		{
		}
	}
}
