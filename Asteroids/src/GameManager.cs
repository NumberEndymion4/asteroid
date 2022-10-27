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
		private readonly CollisionManager collisionManager;
		private readonly Config config;
		private readonly Random random;

		private IGameEnvironment environment;

		public GameManager(Config gameConfig)
		{
			gameObjects = new List<IGameObject>();
			presenters = new List<Func<IPresenter>>();
			collisionManager = new CollisionManager();
			config = gameConfig;
			random = new Random();
		}

		public void Initialize(IGameEnvironment gameEnvironment)
		{
			environment = gameEnvironment;
			var keys = environment.GetKeyStateProvider();
			var screenCenter = config.WindowSize.ToVector2() / 2;

			for (int i = 0; i < 50; ++i) {
				var asteroid = new GameObject {
					Position = screenCenter,
					Scale = random.NextSingle(0.1f, 1f),
				};

				float radPerSec =
					(random.Next(2) == 0 ? -1 : 1) *
					random.NextSingle(MathF.PI / 24, MathF.PI / 16);

				asteroid.AddBehavior(new LinearRotation(asteroid, radPerSec));

				var direction = Vector2.Transform(
					Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
				);

				asteroid.AddBehavior(
					new LinearMovement(asteroid, direction, random.NextSingle(5f, 20f))
				);

				var asteroidCollider = new CircleCollider(asteroid, 65 / 2f);
				asteroid.AddBehavior(asteroidCollider);
				collisionManager.Register(asteroidCollider);

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

			var positionProvider = new PositionProvider(spaceship);
			spaceship.AddBehavior(positionProvider);
			presenters.Add(() => environment.GetSpaceshipPositionToHudPresenter(positionProvider));

			var angleProvider = new AngleProvider(spaceship);
			spaceship.AddBehavior(angleProvider);
			presenters.Add(() => environment.GetSpaceshipAngleToHudPresenter(angleProvider));

			var spaceshipCollider = new CircleCollider(spaceship, 55 / 2f);
			spaceship.AddBehavior(spaceshipCollider);
			collisionManager.Register(spaceshipCollider);
			presenters.Add(() => environment.GetBoundsPresenter(spaceshipCollider));

			gameObjects.Add(spaceship);
			presenters.Add(() => environment.GetSpaceshipPresenter(spaceship));
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in gameObjects) {
				gameObject.Update(gameTime);
			}

			collisionManager.Update(gameTime);
		}

		public void Render(SpriteBatch spriteBatch)
		{
			foreach (var getPresenter in presenters) {
				getPresenter().Render(spriteBatch);
			}
		}
	}
}
