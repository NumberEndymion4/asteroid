using System;
using System.Collections.Generic;
using Asteroids.Components;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
	public class GameManager
	{
		private readonly IGameEnvironment environment;
		private readonly List<IGameObject> gameObjects;
		private readonly List<IGameObject> pendingObjects;
		private readonly List<IPresenter> presenters;
		private readonly List<IPresenter> pendingPresenters;
		private readonly Random random;

		private bool isSceneReady;

		public GameManager(IGameEnvironment gameEnvironment)
		{
			environment = gameEnvironment;
			gameObjects = new List<IGameObject>();
			pendingObjects = new List<IGameObject>();
			presenters = new List<IPresenter>();
			pendingPresenters = new List<IPresenter>();
			random = new Random();
		}

		public void EnsureScene()
		{
			if (isSceneReady) {
				return;
			}

			var keys = environment.GetKeyStateProvider();

			var screenCenter = new Vector2(
				Config.Instance.WindowWidth / 2f, Config.Instance.WindowHeight / 2f
			);

			for (int i = 0; i < Config.Instance.AsteroidCount; ++i) {
				var asteroid = new GameObject {
					Position = screenCenter,
					Scale = random.NextSingle(0.1f, 1f),
				};

				var direction = Vector2.Transform(
					Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
				);

				float radianPerSecond =
					random.NextSign() * random.NextSingle(MathF.PI / 24, MathF.PI / 16);

				var asteroidCollider = new CircleCollider(
					asteroid, Config.Instance.AsteroidGroup, Config.Instance.AsteroidRadius
				);

				asteroid.AddComponent(new LinearRotation(asteroid, radianPerSecond));
				asteroid.AddComponent(new HealthProvider(asteroid, 1));

				float asteroidSpeed = random.NextSingle(
					Config.Instance.AsteroidMinSpeed, Config.Instance.AsteroidMaxSpeed
				);

				asteroid.AddComponent(new LinearMovement(asteroid, direction, asteroidSpeed));
				asteroid.AddComponent(asteroidCollider);
				asteroid.AddComponent(
					new MakeDamageOnCollision(asteroid, Config.Instance.AsteroidGroup, 1)
				);
				asteroid.AddComponent(new TakeDamageOnCollision(
					asteroid, Config.Instance.BulletGroup, Config.Instance.LaserGroup
				));

				presenters.Add(environment.GetAsteroidPresenter(asteroid));
				presenters.Add(environment.GetBoundsPresenter(asteroidCollider));
				gameObjects.Add(asteroid);
			}

			var spaceship = new GameObject {
				Position = new Vector2(100, 100),
			};

			var angleProvider = new AngleProvider(spaceship);
			var positionProvider = new PositionProvider(spaceship);
			var spaceshipCollider = new CircleCollider(
				spaceship, Config.Instance.SpaceshipGroup, Config.Instance.SpaceshipRadius
			);

			var speedOptions = new KineticMovement.Settings {
				MaxSpeed = 400,
				Acceleration = 300,
				Deceleration = 200,
			};

			var triggerSettings = new InputTrigger.Settings {
				KeyStateProvider = keys,
				TriggerOn = Keys.Space,
				Interval = TimeSpan.FromMilliseconds(500),
			};

			var spacebarTrigger = new InputTrigger(spaceship, triggerSettings);
			var gun = new SpawnByTrigger(spaceship, spacebarTrigger);
			gun.Spawn += GunFired;

			spaceship.AddComponent(new InputRotation(spaceship, keys, MathF.PI));
			spaceship.AddComponent(new KineticMovement(spaceship, keys, speedOptions));
			spaceship.AddComponent(new HealthProvider(spaceship, 1));
			spaceship.AddComponent(new WrapPositionOutsideScreen(spaceship));
			spaceship.AddComponent(positionProvider);
			spaceship.AddComponent(angleProvider);
			spaceship.AddComponent(spaceshipCollider);
			spaceship.AddComponent(spacebarTrigger);
			spaceship.AddComponent(gun);

			spaceship.AddComponent(
				new TakeDamageOnCollision(spaceship, Config.Instance.AsteroidGroup)
			);

			CreateAvatar(Orientation.Horizontal);
			CreateAvatar(Orientation.Vertical);
			CreateAvatar(Orientation.All);

			presenters.Add(environment.GetSpaceshipPresenter(spaceship));
			presenters.Add(environment.GetBoundsPresenter(spaceshipCollider));
			presenters.Add(environment.GetSpaceshipAngleToHudPresenter(angleProvider));
			presenters.Add(environment.GetSpaceshipPositionToHudPresenter(positionProvider));
			gameObjects.Add(spaceship);

			isSceneReady = true;

			void CreateAvatar(Orientation orientation)
			{
				var avatar = new ScreenMirrorAvatar(spaceship, orientation);
				var avatarCollider = new CircleCollider(
					avatar, Config.Instance.SpaceshipGroup, Config.Instance.SpaceshipRadius
				);
				spaceship.AddComponent(avatar);
				spaceship.AddComponent(avatarCollider);
				presenters.Add(environment.GetSpaceshipPresenter(avatar));
				presenters.Add(environment.GetBoundsPresenter(avatarCollider));
			}
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

		private void GunFired(IComponent sender)
		{
			var position = sender.Owner.Position;
			var rotation = sender.Owner.Rotation;

			var bullet = new GameObject {
				Position = position,
				Rotation = rotation,
			};

			var direction = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(rotation));
			var bulletCollider = new CircleCollider(bullet, Config.Instance.BulletGroup, 20 / 2f);

			bullet.AddComponent(bulletCollider);
			bullet.AddComponent(new HealthProvider(bullet, 1));
			bullet.AddComponent(new LinearMovement(bullet, direction, 800f));
			bullet.AddComponent(new MakeDamageOnCollision(bullet, Config.Instance.BulletGroup, 1));
			bullet.AddComponent(new TakeDamageOnCollision(bullet, Config.Instance.AsteroidGroup));
			bullet.AddComponent(new DieOutsideScreen(bullet));

			pendingPresenters.Add(environment.GetBulletPresenter(bullet));
			pendingPresenters.Add(environment.GetBoundsPresenter(bulletCollider));
			pendingObjects.Add(bullet);
		}
	}
}
