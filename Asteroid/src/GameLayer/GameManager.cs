using System;
using System.Collections.Generic;
using Asteroids.Core;
using Asteroids.GameLayer.Behaviors;
using Asteroids.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.GameLayer
{
	internal class GameManager
	{
		private readonly Dictionary<IGameObject, Func<IPresenter>> presenters;
		private readonly Config config;

		private IEnvironment environment;

		public GameManager(Config gameConfig)
		{
			presenters = new Dictionary<IGameObject, Func<IPresenter>>();
			config = gameConfig;
		}

		public void Initialize(IEnvironment gameEnvironment)
		{
			environment = gameEnvironment;

			for (int i = 0; i < 50; ++i) {
				presenters.Add(CreateAsteroid(), environment.GetAsteroidPresenter);
			}

			presenters.Add(
				CreateSpaceship(environment.GetKeyStateProvider()),
				environment.GetSpaceshipPresenter
			);
		}

		public void Update(GameTime gameTime)
		{
			foreach (var (gameObject, _) in presenters) {
				gameObject.Update(gameTime);
			}
		}

		public void Render(SpriteBatch spriteBatch)
		{
			foreach (var (gameObject, getPresenter) in presenters) {
				getPresenter().Render(spriteBatch, gameObject);
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
			spaceship.Behaviors.Add(new InputRotation(spaceship, keys, MathF.PI));
			spaceship.Behaviors.Add(new KineticMovement(spaceship, keys, speedOptions));

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

			asteroid.Behaviors.Add(new LinearRotation(asteroid, radPerSec));

			var direction = Vector2.Transform(
				Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
			);

			asteroid.Behaviors.Add(new LinearMovement(asteroid, direction, random.NextSingle(5f, 20f)));

			return asteroid;
		}
	}
}
