using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Components;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
	internal class GameObjectFactory
	{
		public delegate void CreateExternalHandler(
			IReadOnlyCollection<IGameObject> createdGameObjects,
			IReadOnlyCollection<IPresenter> createdPresenters
		);

		private static readonly Random random = new Random();

		public event CreateExternalHandler CreateExternal;

		private readonly List<IGameObject> gameObjectBucket;
		private readonly List<IPresenter> presenterBucket;

		public static GameObjectFactory Instance { get; } = new GameObjectFactory();

		private GameObjectFactory()
		{
			gameObjectBucket = new List<IGameObject>();
			presenterBucket = new List<IPresenter>();
		}

		public void CreateSpaceship(
			IGameEnvironment environment,
			out IReadOnlyCollection<IGameObject> gameObjects,
			out IReadOnlyCollection<IPresenter> presenters
		) {
			var spaceship = new GameObject {
				Position = new Vector2(100, 100),
			};

			var angleProvider = new AngleProvider();
			var positionProvider = new PositionProvider();
			var spaceshipCollider = new CircleCollider(
				spaceship, Config.Instance.SpaceshipGroup, Config.Instance.SpaceshipRadius
			);

			var speedOptions = new KineticMovement.Settings {
				MaxSpeed = 400,
				Acceleration = 300,
				Deceleration = 200,
			};

			var keys = environment.GetKeyStateProvider();
			var triggerSettings = new InputTrigger.Settings {
				KeyStateProvider = keys,
				TriggerOn = Keys.Space,
				Interval = TimeSpan.FromMilliseconds(500),
			};

			var spacebarTrigger = new InputTrigger(triggerSettings);
			var gun = new SpawnByTrigger(spacebarTrigger);

			gun.Spawn += (sender, position, rotation) => {
				CreateBullet(
					environment, position, rotation, out var gameObjects, out var bulletPresenters
				);
				CreateExternal?.Invoke(gameObjects, bulletPresenters);
			};

			spaceship.AddComponent(new InputRotation(keys, MathF.PI));
			spaceship.AddComponent(new KineticMovement(keys, speedOptions));
			spaceship.AddComponent(new HealthProvider(1));
			spaceship.AddComponent(new WrapPositionOutsideScreen());
			spaceship.AddComponent(positionProvider);
			spaceship.AddComponent(angleProvider);
			spaceship.AddComponent(spaceshipCollider);
			spaceship.AddComponent(spacebarTrigger);
			spaceship.AddComponent(new DamageAcceptor(Config.Instance.AsteroidGroup));
			spaceship.AddComponent(gun);

			gameObjectBucket.Clear();
			gameObjectBucket.Add(spaceship);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			CreateAvatar(environment, spaceship, Orientation.Horizontal, presenterBucket);
			CreateAvatar(environment, spaceship, Orientation.Vertical, presenterBucket);
			CreateAvatar(environment, spaceship, Orientation.All, presenterBucket);

			presenterBucket.Add(environment.GetSpaceshipPresenter(spaceship));
			presenterBucket.Add(environment.GetBoundsPresenter(spaceshipCollider));
			presenterBucket.Add(environment.GetSpaceshipAngleToHudPresenter(angleProvider));
			presenterBucket.Add(environment.GetSpaceshipPositionToHudPresenter(positionProvider));
			presenters = presenterBucket;
		}

		public void CreateAsteroid(
			IGameEnvironment environment,
			Vector2 position,
			ObstacleSettings settings,
			IEnumerable<ObstacleSettings> partsSettings,
			out IReadOnlyCollection<IGameObject> gameObjects,
			out IReadOnlyCollection<IPresenter> presenters
		) {
			var asteroid = new GameObject {
				Position = position,
				Scale = random.NextSingle(settings.ScaleMin, settings.ScaleMax),
			};

			var direction = Vector2.Transform(
				Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
			);

			float radianPerSecond =
				random.NextSign() *
				random.NextSingle(settings.AngleVelocityMin, settings.AngleVelocityMax);

			var asteroidCollider = new CircleCollider(
				asteroid, Config.Instance.AsteroidGroup, Config.Instance.AsteroidRadius
			);

			float asteroidSpeed = random.NextSingle(
				Config.Instance.AsteroidMinSpeed, Config.Instance.AsteroidMaxSpeed
			);

			var damageAcceptor = new DamageAcceptor(
				Config.Instance.BulletGroup, Config.Instance.LaserGroup
			);

			var spawnByDamage = new SpawnByTrigger(damageAcceptor);

			spawnByDamage.Spawn += (sender, position, rotation) => {
				foreach (var part in partsSettings) {
					CreateAsteroid(
						environment,
						position,
						part,
						Enumerable.Empty<ObstacleSettings>(),
						out var gameObjects,
						out var bulletPresenters
					);

					CreateExternal?.Invoke(gameObjects, bulletPresenters);
				}
			};

			asteroid.AddComponent(new LinearMovement(direction, asteroidSpeed));
			asteroid.AddComponent(new LinearRotation(radianPerSecond));
			asteroid.AddComponent(new HealthProvider(1));
			asteroid.AddComponent(new DieOutsideScreen());
			asteroid.AddComponent(new PositionProvider());
			asteroid.AddComponent(asteroidCollider);
			asteroid.AddComponent(new DamageProvider(1));
			asteroid.AddComponent(damageAcceptor);
			asteroid.AddComponent(spawnByDamage);

			gameObjectBucket.Clear();
			gameObjectBucket.Add(asteroid);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			presenterBucket.Add(environment.GetAsteroidPresenter(asteroid));
			presenterBucket.Add(environment.GetBoundsPresenter(asteroidCollider));
			presenters = presenterBucket;
		}

		private void CreateBullet(
			IGameEnvironment environment,
			Vector2 position,
			float rotation,
			out IReadOnlyCollection<IGameObject> gameObjects,
			out IReadOnlyCollection<IPresenter> presenters
		) {
			var bullet = new GameObject {
				Position = position,
				Rotation = rotation,
			};

			var direction = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(rotation));
			var bulletCollider = new CircleCollider(bullet, Config.Instance.BulletGroup, 20 / 2f);

			bullet.AddComponent(bulletCollider);
			bullet.AddComponent(new HealthProvider(1));
			bullet.AddComponent(new LinearMovement(direction, 800f));
			bullet.AddComponent(new DamageProvider(1));
			bullet.AddComponent(new DamageAcceptor(Config.Instance.AsteroidGroup));
			bullet.AddComponent(new DieOutsideScreen());

			gameObjectBucket.Clear();
			gameObjectBucket.Add(bullet);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			presenterBucket.Add(environment.GetBulletPresenter(bullet));
			presenterBucket.Add(environment.GetBoundsPresenter(bulletCollider));
			presenters = presenterBucket;
		}

		private static IGameObject CreateAvatar(
			IGameEnvironment environment,
			IGameObject source,
			Orientation orientation,
			List<IPresenter> presenters
		) {
			var avatar = new ScreenMirrorAvatar(source, orientation);
			var avatarCollider = new CircleCollider(
				avatar, Config.Instance.SpaceshipGroup, Config.Instance.SpaceshipRadius
			);

			source.AddComponent(avatar);
			source.AddComponent(avatarCollider);
			presenters.Add(environment.GetSpaceshipPresenter(avatar));
			presenters.Add(environment.GetBoundsPresenter(avatarCollider));

			return source;
		}
	}
}
