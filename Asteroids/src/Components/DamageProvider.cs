using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class DamageProvider : IDataProvider<int>
	{
		public int Damage { get; }

		int IDataProvider<int>.Data => Damage;

		public DamageProvider(int damage)
		{
			Damage = damage;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
		}
	}
}
