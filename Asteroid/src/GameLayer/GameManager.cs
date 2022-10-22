using System;
using System.Collections.Generic;
using Asteroids.Core;
using Asteroids.GameLayer.Behaviors;
using Asteroids.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.GameLayer
{
	internal class GameManager
	{
		private readonly List<IGameObject> gameObjects;
		private readonly List<Func<IPresenter>> presenters;
		private readonly Config config;

		private IEnvironment environment;

		public GameManager(Config gameConfig)
		{
			gameObjects = new List<IGameObject>();
			presenters = new List<Func<IPresenter>>();
			config = gameConfig;
		}

		public void Initialize(IEnvironment gameEnvironment)
		{
			environment = gameEnvironment;

			for (int i = 0; i < 50; ++i) {
				var asteroid = CreateAsteroid();
				gameObjects.Add(asteroid);
				presenters.Add(() => environment.GetAsteroidPresenter(asteroid));
			}

			var spaceship = CreateSpaceship(environment.GetKeyStateProvider());
			gameObjects.Add(spaceship);
			presenters.Add(() => environment.GetSpaceshipPresenter(spaceship));
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in gameObjects) {
				gameObject.Update(gameTime);
			}
		}

		public void Render(SpriteBatch spriteBatch)
		{
			foreach (var getPresenter in presenters) {
				getPresenter().Render(spriteBatch);
			}
		}

		private IGameObject CreateSpaceship(IKeyStateProvider keys)
		{
			var speedOptions = new KineticMovement.Options {
				MaxSpeed = 400,
				Acceleration = 300,
				Deceleration = 200
			};

			var spaceship = new GameObject {
				Position = config.WindowSize.ToVector2() / 2
			};
			spaceship.Behaviors.Add(new InputRotation(keys, MathF.PI));
			spaceship.Behaviors.Add(new KineticMovement(keys, speedOptions));

			return spaceship;
		}

		private IGameObject CreateAsteroid()
		{
			var random = RandomExtension.GlobalRandom;

			var asteroid = new GameObject {
				Position = config.WindowSize.ToVector2() / 2,
				Scale = random.NextSingle(0.1f, 1f)
			};

			float radPerSec =
				(random.Next(2) == 0 ? -1 : 1) *
				random.NextSingle(MathF.PI / 24, MathF.PI / 16);

			asteroid.Behaviors.Add(new LinearRotation(radPerSec));

			var direction = Vector2.Transform(
				Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
			);

			asteroid.Behaviors.Add(new LinearMovement(direction, random.NextSingle(5f, 20f)));

			return asteroid;
		}
	}
}
