using Asteroids.Broadcast;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class SpawnAsteroidOnHealth : IComponent
	{
		private readonly int spawnOnHeath;
		private readonly int spawnCount;

		public SpawnAsteroidOnHealth(int health, int count)
		{
			spawnOnHeath = health;
			spawnCount = count;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (gameObject.GetComponent<HealthProvider>()?.Health == spawnOnHeath) {
				for (int i = 0; i < spawnCount; ++i) {
					BroadcastService.Instance.Schedule(
						new GameObjectMessage("spawn_asteroid", gameObject)
					);
				}
			}
		}
	}
}
