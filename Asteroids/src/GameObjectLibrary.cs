using System;
using System.Collections.Generic;
using Asteroids.Components;
using Core;
using Core.Collisions;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	internal class GameObjectFactory
	{
		private static readonly Random random = new Random();

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

			var movement = new KineticMovement(
				new KineticMovement.Settings {
					MaxSpeed = 400,
					Acceleration = 300,
					Deceleration = 200,
				}
			);

			var fireBullet = new IntervalBroadcastListener(
				"spawn_bullet",
				"start_shoot1",
				"stop_shoot1",
				Config.Instance.BulletFireRate,
				resetInterval: false
			);

			var fireLaser = new IntervalBroadcastListener(
				"spawn_laser",
				"start_shoot2",
				"stop_shoot2",
				Config.Instance.LaserFireRate,
				resetInterval: true
			);

			spaceship.AddComponent(new InputRotation(MathF.PI));
			spaceship.AddComponent(movement);
			spaceship.AddComponent(new HealthProvider(1));
			spaceship.AddComponent(new WrapPositionOutsideScreen());
			spaceship.AddComponent(positionProvider);
			spaceship.AddComponent(angleProvider);
			spaceship.AddComponent(spaceshipCollider);
			spaceship.AddComponent(new SuicideOnCollision(
				Config.Instance.AsteroidGroup, Config.Instance.UfoGroup
			));
			spaceship.AddComponent(fireBullet);
			spaceship.AddComponent(fireLaser);

			gameObjectBucket.Clear();
			gameObjectBucket.Add(spaceship);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			CreateSpaceshipAvatar(environment, spaceship, Orientation.Horizontal, presenterBucket);
			CreateSpaceshipAvatar(environment, spaceship, Orientation.Vertical, presenterBucket);
			CreateSpaceshipAvatar(environment, spaceship, Orientation.All, presenterBucket);

			presenterBucket.Add(environment.GetSpaceshipPresenter(spaceship));
			presenterBucket.Add(environment.GetCircleColliderPresenter(spaceshipCollider));
			presenterBucket.Add(environment.GetSpaceshipAngleToHudPresenter(angleProvider));
			presenterBucket.Add(environment.GetSpaceshipPositionToHudPresenter(positionProvider));
			presenterBucket.Add(environment.GetSpaceshipSpeedToHudPresenter(movement));
			presenterBucket.Add(environment.GetLaserCooldownToHudPresenter(fireLaser));
			presenters = presenterBucket;
		}

		public void CreateUfo(
			IGameEnvironment environment,
			Vector2 position,
			UfoSettings settings,
			out IReadOnlyCollection<IGameObject> gameObjects,
			out IReadOnlyCollection<IPresenter> presenters
		) {
			var ufo = new GameObject {
				Position = position,
			};

			var direction = Vector2.Transform(
				Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
			);

			var ufoCollider = new CircleCollider(
				ufo, Config.Instance.UfoGroup, Config.Instance.UfoRadius
			);

			float ufoSpeed = random.NextSingle(
				Config.Instance.UfoMinSpeed, Config.Instance.UfoMaxSpeed
			);

			ufo.AddComponent(new LinearMovement(direction, ufoSpeed));
			ufo.AddComponent(new SuicideOutsideScreen());
			ufo.AddComponent(ufoCollider);
			ufo.AddComponent(new DamageProvider(1));
			ufo.AddComponent(new SuicideOnCollision(
				Config.Instance.BulletGroup, Config.Instance.LaserGroup
			));
			ufo.AddComponent(new HealthProvider(1));
			ufo.AddComponent(new ScoreProvider(settings.Score));

			gameObjectBucket.Clear();
			gameObjectBucket.Add(ufo);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			presenterBucket.Add(environment.GetUfoPresenter(ufo));
			presenterBucket.Add(environment.GetCircleColliderPresenter(ufoCollider));
			presenters = presenterBucket;
		}

		public void CreateAsteroid(
			IGameEnvironment environment,
			Vector2 position,
			AsteroidSettings settings,
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

			asteroid.AddComponent(new LinearMovement(direction, asteroidSpeed));
			asteroid.AddComponent(new LinearRotation(radianPerSecond));
			asteroid.AddComponent(new SuicideOutsideScreen());
			asteroid.AddComponent(new PositionProvider());
			asteroid.AddComponent(asteroidCollider);
			asteroid.AddComponent(new DamageProvider(1));
			asteroid.AddComponent(new TakeDamageOnCollision(Config.Instance.BulletGroup));
			asteroid.AddComponent(new SuicideOnCollision(Config.Instance.LaserGroup));
			asteroid.AddComponent(new HealthProvider(1));
			asteroid.AddComponent(new SpawnAsteroidOnHealth(0, settings.PartsCount));
			asteroid.AddComponent(new ScoreProvider(settings.Score));

			gameObjectBucket.Clear();
			gameObjectBucket.Add(asteroid);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			presenterBucket.Add(environment.GetAsteroidPresenter(asteroid));
			presenterBucket.Add(environment.GetCircleColliderPresenter(asteroidCollider));
			presenters = presenterBucket;
		}

		public void CreateBullet(
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
			bullet.AddComponent(new SuicideOnCollision(
				Config.Instance.AsteroidGroup, Config.Instance.UfoGroup
			));
			bullet.AddComponent(new SuicideOutsideScreen());

			gameObjectBucket.Clear();
			gameObjectBucket.Add(bullet);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			presenterBucket.Add(environment.GetBulletPresenter(bullet));
			presenterBucket.Add(environment.GetCircleColliderPresenter(bulletCollider));
			presenters = presenterBucket;
		}

		public void CreateLaser(
			IGameEnvironment environment,
			Vector2 position,
			float rotation,
			out IReadOnlyCollection<IGameObject> gameObjects,
			out IReadOnlyCollection<IPresenter> presenters
		) {
			var laser = new GameObject {
				Position = position,
				Rotation = rotation,
			};

			var length = Config.Instance.ScreenRect.Size.ToVector2().Length();
			var direction = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(rotation));

			var collider = new LineCollider(
				laser, Config.Instance.LaserGroup, position, position + direction * length
			);

			laser.AddComponent(new HealthProvider(1));
			laser.AddComponent(new SuicideOnTime(TimeSpan.Zero));
			laser.AddComponent(new DamageProvider(1));
			laser.AddComponent(collider);

			gameObjectBucket.Clear();
			gameObjectBucket.Add(laser);
			gameObjects = gameObjectBucket;

			presenterBucket.Clear();
			presenterBucket.Add(environment.GetLaserPresenter(laser));
			presenters = presenterBucket;
		}

		private static IGameObject CreateSpaceshipAvatar(
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
			presenters.Add(environment.GetCircleColliderPresenter(avatarCollider));

			return source;
		}
	}
}
