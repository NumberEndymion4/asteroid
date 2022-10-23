using System;
using System.Collections.Generic;
using Asteroids.GameLayer.Behaviors;
using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameLayer
{
	internal class GameManager
	{
		private readonly List<IGameObject> gameObjects;
		private readonly List<Func<IPresenter>> presenters;
		private readonly CollisionManager collisionManager;
		private readonly Config config;
		private readonly Random random;

		private IEnvironment environment;

		public GameManager(Config gameConfig)
		{
			gameObjects = new List<IGameObject>();
			presenters = new List<Func<IPresenter>>();
			collisionManager = new CollisionManager();
			config = gameConfig;
			random = new Random();
		}

		public void Initialize(IEnvironment gameEnvironment)
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

				asteroid.Behaviors.Add(new LinearRotation(radPerSec));

				var direction = Vector2.Transform(
					Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
				);

				asteroid.Behaviors.Add(new LinearMovement(direction, random.NextSingle(5f, 20f)));

				var asteroidCollider = new CircleCollider(65 / 2f);
				asteroid.Behaviors.Add(asteroidCollider);
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
			spaceship.Behaviors.Add(new InputRotation(keys, MathF.PI));
			spaceship.Behaviors.Add(new KineticMovement(keys, speedOptions));

			var positionProvider = new PositionProvider();
			spaceship.Behaviors.Add(positionProvider);
			presenters.Add(() => environment.GetSpaceshipPositionToHudPresenter(positionProvider));

			var angleProvider = new AngleProvider();
			spaceship.Behaviors.Add(angleProvider);
			presenters.Add(() => environment.GetSpaceshipAngleToHudPresenter(angleProvider));

			var spaceshipCollider = new CircleCollider(55 / 2f);
			spaceship.Behaviors.Add(spaceshipCollider);
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
