using System;
using System.Collections.Generic;
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
		)
		{
			var spaceship = new GameObject {
				Position = new Vector2(100, 100),
			};

			var angleProvider = new AngleProvider(spaceship);
			var positionProvider = new PositionProvider(spaceship);
			var spaceshipCollider = new CircleCollider(
				spaceship, Config.Instance.SpaceshipGroup, Config.Instance.SpaceshipRadius
			);

			var damageOnCollision = new TakeDamageOnCollision(
				spaceship, Config.Instance.AsteroidGroup
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

			var spacebarTrigger = new InputTrigger(spaceship, triggerSettings);
			var gun = new SpawnByTrigger(spaceship, spacebarTrigger);

			gun.Spawn += sender => {
				CreateBullet(environment, sender, out var gameObjects, out var bulletPresenters);
				CreateExternal?.Invoke(gameObjects, bulletPresenters);
			};

			spaceship.AddComponent(new InputRotation(spaceship, keys, MathF.PI));
			spaceship.AddComponent(new KineticMovement(spaceship, keys, speedOptions));
			spaceship.AddComponent(new HealthProvider(spaceship, 1));
			spaceship.AddComponent(new WrapPositionOutsideScreen(spaceship));
			spaceship.AddComponent(positionProvider);
			spaceship.AddComponent(angleProvider);
			spaceship.AddComponent(spaceshipCollider);
			spaceship.AddComponent(spacebarTrigger);
			spaceship.AddComponent(damageOnCollision);
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
			out IReadOnlyCollection<IGameObject> gameObjects,
			out IReadOnlyCollection<IPresenter> presenters
		)
		{
			var asteroid = new GameObject {
				Position = Config.Instance.ScreenRect.Center.ToVector2(),
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

			float asteroidSpeed = random.NextSingle(
				Config.Instance.AsteroidMinSpeed, Config.Instance.AsteroidMaxSpeed
			);

			var makeDamageOnCollision = new MakeDamageOnCollision(
				asteroid, Config.Instance.AsteroidGroup, 1
			);

			var takeDamageOnCollision = new TakeDamageOnCollision(
				asteroid, Config.Instance.BulletGroup, Config.Instance.LaserGroup
			);

			asteroid.AddComponent(new LinearMovement(asteroid, direction, asteroidSpeed));
			asteroid.AddComponent(new LinearRotation(asteroid, radianPerSecond));
			asteroid.AddComponent(new HealthProvider(asteroid, 1));
			asteroid.AddComponent(new DieOutsideScreen(asteroid));
			asteroid.AddComponent(asteroidCollider);
			asteroid.AddComponent(makeDamageOnCollision);
			asteroid.AddComponent(takeDamageOnCollision);

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
			IComponent sender,
			out IReadOnlyCollection<IGameObject> gameObjects,
			out IReadOnlyCollection<IPresenter> presenters
		)
		{
			float rotation = sender.Owner.Rotation;
			var bullet = new GameObject {
				Position = sender.Owner.Position,
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
		)
		{
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
