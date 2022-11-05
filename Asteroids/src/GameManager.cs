﻿using System.Collections.Generic;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
	public class GameManager
	{
		private readonly IGameEnvironment environment;
		private readonly List<IGameObject> gameObjects;
		private readonly List<IGameObject> pendingObjects;
		private readonly List<IPresenter> presenters;
		private readonly List<IPresenter> pendingPresenters;

		private bool isSceneReady;

		public GameManager(IGameEnvironment gameEnvironment)
		{
			environment = gameEnvironment;
			gameObjects = new List<IGameObject>();
			pendingObjects = new List<IGameObject>();
			presenters = new List<IPresenter>();
			pendingPresenters = new List<IPresenter>();

			GameObjectFactory.Instance.CreateExternal += OnCreateExternal;
		}

		public void EnsureScene()
		{
			if (isSceneReady) {
				return;
			}

			var newObjects = default(IReadOnlyCollection<IGameObject>);
			var newPresenters = default(IReadOnlyCollection<IPresenter>);

			for (int i = 0; i < Config.Instance.AsteroidCount; ++i) {
				GameObjectFactory.Instance.CreateAsteroid(
					environment, out newObjects, out newPresenters
				);
				gameObjects.AddRange(newObjects);
				presenters.AddRange(newPresenters);
			}

			GameObjectFactory.Instance.CreateSpaceship(
				environment, out newObjects, out newPresenters
			);
			gameObjects.AddRange(newObjects);
			presenters.AddRange(newPresenters);

			isSceneReady = true;
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in gameObjects) {
				gameObject.Update(gameTime);
			}

			for (int i = gameObjects.Count - 1; i >= 0; --i) {
				var gameObject = gameObjects[i];

				if (gameObject.IsDead() && gameObject.IsComplete()) {
					gameObject.Dispose();
					gameObjects.RemoveAt(i);
				}
			}

			CollisionService.Instance.Update(gameTime);

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

		private void OnCreateExternal(
			IReadOnlyCollection<IGameObject> createdGameObjects,
			IReadOnlyCollection<IPresenter> createdPresenters
		)
		{
			pendingObjects.AddRange(createdGameObjects);
			pendingPresenters.AddRange(createdPresenters);
		}
	}
}
