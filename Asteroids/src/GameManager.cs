using System.Collections.Generic;
using Asteroids.Broadcast;
using Asteroids.Components;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
	public partial class GameManager : Disposable, IBroadcastListener
	{
		private readonly IGameEnvironment environment;
		private readonly List<IGameObject> gameObjects;
		private readonly List<IGameObject> pendingObjects;
		private readonly List<IPresenter> presenters;
		private readonly List<IPresenter> pendingPresenters;
		private readonly ScoresProvider scoresProvider;

		private bool isSceneReady;
		private int totalScores;

		public GameManager(IGameEnvironment gameEnvironment)
		{
			environment = gameEnvironment;
			gameObjects = new List<IGameObject>();
			pendingObjects = new List<IGameObject>();
			presenters = new List<IPresenter>();
			pendingPresenters = new List<IPresenter>();
			scoresProvider = new ScoresProvider(this);

			BroadcastService.Instance.Register(this);
		}

		public void EnsureScene()
		{
			if (isSceneReady) {
				return;
			}

			IReadOnlyCollection<IGameObject> newObjects;
			IReadOnlyCollection<IPresenter> newPresenters;

			for (int i = 0; i < Config.Instance.AsteroidCount; ++i) {
				GameObjectFactory.Instance.CreateAsteroid(
					environment,
					Config.Instance.ScreenRect.Center.ToVector2(),
					Config.Instance.BigAsteroid,
					out newObjects,
					out newPresenters
				);
				gameObjects.AddRange(newObjects);
				presenters.AddRange(newPresenters);
			}

			for (int i = 0; i < Config.Instance.UfoCount; ++i) {
				GameObjectFactory.Instance.CreateUfo(
					environment,
					Config.Instance.ScreenRect.Center.ToVector2(),
					Config.Instance.RegularUfoSettings,
					out newObjects,
					out newPresenters
				);
				gameObjects.AddRange(newObjects);
				presenters.AddRange(newPresenters);
			}

			GameObjectFactory.Instance.CreateSpaceship(
				environment, out newObjects, out newPresenters
			);
			gameObjects.AddRange(newObjects);
			presenters.AddRange(newPresenters);

			presenters.Add(environment.GetScoresToHudPresenter(scoresProvider));
			isSceneReady = true;
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in gameObjects) {
				gameObject.Update(gameTime);
			}

			CollisionService.Instance.Update();
			BroadcastService.Instance.Update(gameTime);

			for (int i = gameObjects.Count - 1; i >= 0; --i) {
				var gameObject = gameObjects[i];
				if (gameObject.IsDead() && gameObject.IsComplete()) {
					gameObject.Dispose();
					gameObjects.RemoveAt(i);
				}
			}

			for (int i = presenters.Count - 1; i >= 0; --i) {
				if (presenters[i].IsTargetLost) {
					presenters.RemoveAt(i);
				}
			}

			gameObjects.AddRange(pendingObjects);
			pendingObjects.Clear();

			presenters.AddRange(pendingPresenters);
			pendingPresenters.Clear();
		}

		public void Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
			foreach (var presenter in presenters) {
				presenter.Render(spriteBatch, gameTime);
			}
		}

		protected override void PerformDispose()
		{
			BroadcastService.Instance.Unregister(this);
		}

		void IBroadcastListener.Notify(IBroadcastMessage message, GameTime gameTime)
		{
			if (message is GameObjectMessage { Tag: "spawn_bullet" } spawnBullet) {
				GameObjectFactory.Instance.CreateBullet(
					environment,
					spawnBullet.Sender.GetComponent<PositionProvider>()?.Position ?? Vector2.Zero,
					spawnBullet.Sender.GetComponent<AngleProvider>()?.Radians ?? 0f,
					out var bulletObjects,
					out var bulletPresenters
				);
				pendingObjects.AddRange(bulletObjects);
				pendingPresenters.AddRange(bulletPresenters);
			} else if (message is GameObjectMessage { Tag: "spawn_laser" } spawnLaser) {
				GameObjectFactory.Instance.CreateLaser(
					environment,
					spawnLaser.Sender.GetComponent<PositionProvider>()?.Position ?? Vector2.Zero,
					spawnLaser.Sender.GetComponent<AngleProvider>()?.Radians ?? 0f,
					out var laserObjects,
					out var laserPresenters
				);
				pendingObjects.AddRange(laserObjects);
				pendingPresenters.AddRange(laserPresenters);
			} else if (message is GameObjectMessage { Tag: "spawn_asteroid" } spawnAsteroid) {
				GameObjectFactory.Instance.CreateAsteroid(
					environment,
					spawnAsteroid.Sender.GetComponent<PositionProvider>()?.Position ?? Vector2.Zero,
					Config.Instance.SmallAsteroid,
					out var asteroidObjects,
					out var asteroidPresenters
				);
				pendingObjects.AddRange(asteroidObjects);
				pendingPresenters.AddRange(asteroidPresenters);
			} else if (message is ScoreMessage { Tag: "scores" } addScore) {
				totalScores += addScore.Scores;
			}
		}
	}
}
