using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class SpawnByTrigger : Disposable, IComponent, ISpawner
	{
		public event SpawnHandler Spawn;

		private ITrigger trigger;
		private bool isTriggered;

		public SpawnByTrigger(ITrigger spawnTrigger)
		{
			trigger = spawnTrigger;
			trigger.Triggered += OnTriggered;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (isTriggered) {
				Spawn?.Invoke(
					this,
					gameObject.GetComponent<PositionProvider>()?.Position ?? Vector2.Zero,
					gameObject.GetComponent<AngleProvider>()?.Radians ?? 0f
				);
			}
			isTriggered = false;
		}

		protected override void PerformDispose()
		{
			trigger.Triggered -= OnTriggered;
			trigger = null;
			Spawn = null;
			base.PerformDispose();
		}

		private void OnTriggered()
		{
			isTriggered = true;
		}
	}
}
