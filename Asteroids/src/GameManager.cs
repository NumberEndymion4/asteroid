using System;
using System.Collections.Generic;
using Asteroids.Components;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
	public class GameManager
	{
		private readonly List<IGameObject> gameObjects;
		private readonly List<IPresenter> presenters;
		private readonly Random random;

		public bool IsSceneReady { get; private set; }

		public GameManager()
		{
			gameObjects = new List<IGameObject>();
			presenters = new List<IPresenter>();
			random = new Random();
		}

		public void PrepareScene(IGameEnvironment environment)
		{
			var keys = environment.GetKeyStateProvider();

			var screenCenter = new Vector2(
				Config.Instance.WindowWidth / 2f, Config.Instance.WindowHeight / 2f
			);

			for (int i = 0; i < 20; ++i) {
				var asteroid = new GameObject {
					Position = screenCenter,
					Scale = random.NextSingle(0.1f, 1f),
				};

				var direction = Vector2.Transform(
					Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
				);

				float radianPerSecond =
					random.NextSign() * random.NextSingle(MathF.PI / 24, MathF.PI / 16);

				var asteroidCollider = new CircleCollider(asteroid, 65 / 2f);

				asteroid.AddComponent(new LinearRotation(asteroid, radianPerSecond));
				asteroid.AddComponent(new HealthProvider(asteroid, 1));

				asteroid.AddComponent(
					new LinearMovement(asteroid, direction, random.NextSingle(5f, 15f))
				);

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
			var spaceshipCollider = new CircleCollider(spaceship, 55 / 2f);

			var speedOptions = new KineticMovement.Options {
				MaxSpeed = 400,
				Acceleration = 300,
				Deceleration = 200,
			};

			spaceship.AddComponent(new InputRotation(spaceship, keys, MathF.PI));
			spaceship.AddComponent(new KineticMovement(spaceship, keys, speedOptions));
			spaceship.AddComponent(new HealthProvider(spaceship, 1));

			spaceship.AddComponent(positionProvider);
			spaceship.AddComponent(angleProvider);

			spaceship.AddComponent(spaceshipCollider);
			spaceship.AddComponent(
				new TakeDamageOnCollision(spaceship, Config.Instance.AsteroidGroup)
			);

			presenters.Add(environment.GetSpaceshipPresenter(spaceship));
			presenters.Add(environment.GetBoundsPresenter(spaceshipCollider));
			presenters.Add(environment.GetSpaceshipAngleToHudPresenter(angleProvider));
			presenters.Add(environment.GetSpaceshipPositionToHudPresenter(positionProvider));
			gameObjects.Add(spaceship);

			IsSceneReady = true;
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in gameObjects) {
				gameObject.Update(gameTime);
			}

			CollisionService.Instance.Update(gameTime);
		}

		public void Render(SpriteBatch spriteBatch, GameTime gameTime)
		{
			foreach (var presenter in presenters) {
				presenter.Render(spriteBatch, gameTime);
			}
		}
	}
}
