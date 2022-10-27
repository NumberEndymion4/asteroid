using System;
using System.Collections.Generic;
using Asteroids.Behaviors;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
	public class GameManager
	{
		private readonly List<IGameObject> gameObjects;
		private readonly List<Func<IPresenter>> presenters;
		private readonly Random random;

		public GameManager()
		{
			gameObjects = new List<IGameObject>();
			presenters = new List<Func<IPresenter>>();
			random = new Random();
		}

		public void Initialize(IGameEnvironment environment)
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

				float radPerSec =
					(random.Next(2) == 0 ? -1 : 1) *
					random.NextSingle(MathF.PI / 24, MathF.PI / 16);

				asteroid.AddBehavior(new LinearRotation(asteroid, radPerSec));
				asteroid.AddBehavior(new HealthProvider(asteroid, 1));

				var direction = Vector2.Transform(
					Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
				);

				asteroid.AddBehavior(
					new LinearMovement(asteroid, direction, random.NextSingle(5f, 15f))
				);

				var asteroidCollider = new CircleCollider(asteroid, 65 / 2f);
				asteroid.AddBehavior(asteroidCollider);
				asteroid.AddBehavior(
					new MakeDamageOnCollision(asteroid, Config.Instance.AsteroidGroup, 1)
				);
				asteroid.AddBehavior(new TakeDamageOnCollision(
					asteroid, new[] { Config.Instance.BulletGroup, Config.Instance.LaserGroup }
				));

				gameObjects.Add(asteroid);
				presenters.Add(() => environment.GetAsteroidPresenter(asteroid));
				presenters.Add(() => environment.GetBoundsPresenter(asteroidCollider));
			}

			var speedOptions = new KineticMovement.Options {
				MaxSpeed = 400,
				Acceleration = 300,
				Deceleration = 200,
			};

			var spaceship = new GameObject {
				Position = new Vector2(100, 100),
			};
			spaceship.AddBehavior(new InputRotation(spaceship, keys, MathF.PI));
			spaceship.AddBehavior(new KineticMovement(spaceship, keys, speedOptions));
			spaceship.AddBehavior(new HealthProvider(spaceship, 1));

			var positionProvider = new PositionProvider(spaceship);
			spaceship.AddBehavior(positionProvider);
			presenters.Add(() => environment.GetSpaceshipPositionToHudPresenter(positionProvider));

			var angleProvider = new AngleProvider(spaceship);
			spaceship.AddBehavior(angleProvider);
			presenters.Add(() => environment.GetSpaceshipAngleToHudPresenter(angleProvider));

			var spaceshipCollider = new CircleCollider(spaceship, 55 / 2f);
			spaceship.AddBehavior(spaceshipCollider);
			spaceship.AddBehavior(
				new TakeDamageOnCollision(spaceship, new[] { Config.Instance.AsteroidGroup })
			);

			gameObjects.Add(spaceship);
			presenters.Add(() => environment.GetSpaceshipPresenter(spaceship));
			presenters.Add(() => environment.GetBoundsPresenter(spaceshipCollider));
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in gameObjects) {
				gameObject.Update(gameTime);
			}

			CollisionService.Instance.Update(gameTime);
		}

		public void Render(SpriteBatch spriteBatch)
		{
			foreach (var getPresenter in presenters) {
				getPresenter().Render(spriteBatch);
			}
		}
	}
}
