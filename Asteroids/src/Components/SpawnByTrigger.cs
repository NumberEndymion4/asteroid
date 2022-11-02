using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class SpawnByTrigger : Component, ISpawner
	{
		public event SpawnHandler Spawn;

		private ITrigger trigger;
		private bool isTriggered;

		public SpawnByTrigger(IGameObject owner, ITrigger spawnTrigger) : base(owner)
		{
			trigger = spawnTrigger;
			trigger.Triggered += OnTriggered;
		}

		public override void Update(GameTime gameTime)
		{
			if (isTriggered) {
				Spawn?.Invoke(this);
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
