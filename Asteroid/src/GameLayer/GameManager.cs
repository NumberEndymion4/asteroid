using System;
using System.Collections.Generic;
using Asteroids.Core;
using Asteroids.GameLayer.Behaviors;
using Asteroids.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer
{
	internal class Spaceship : GameObject { }
	internal class Asteroid : GameObject { }

	internal class GameManager
	{
		private readonly List<IGameObject> backgroundItems;
		private readonly List<Asteroid> asteroids;

		private readonly Config config;
		private readonly IKeyStateProvider keys;

		private Spaceship spaceship;

		public GameManager(Config instance, IKeyStateProvider keyStateProvider)
		{
			config = instance;
			keys = keyStateProvider;

			backgroundItems = new List<IGameObject>();
			asteroids = new List<Asteroid>();
		}

		public void Initialize()
		{
			var random = new Random();
			var speedOptions = new KineticMovement.Options {
				MaxSpeed = 400,
				Acceleration = 300,
				Deceleration = 200
			};

			var windowCenter = new Vector2(config.WindowWidth / 2, config.WindowHeight / 2);
			spaceship = new Spaceship { Position = windowCenter };
			spaceship.Behaviors.Add(new InputRotation(spaceship, keys, MathF.PI));
			spaceship.Behaviors.Add(new KineticMovement(spaceship, keys, speedOptions));

			for (int i = 0; i < 50; ++i) {
				var asteroid = new Asteroid {
					Position = windowCenter,
					Scale = random.NextSingle(0.1f, 1f)
				};

				float radPerSec =
					(random.Next(2) == 0 ? -1 : 1) *
					random.NextSingle(MathF.PI / 24, MathF.PI / 16);

				asteroid.Behaviors.Add(new LinearRotation(asteroid, radPerSec));

				var direction = Vector2.Transform(
					Vector2.UnitX, Matrix.CreateRotationZ(random.NextSingle() * MathF.Tau)
				);

				asteroid.Behaviors.Add(
					new LinearMovement(asteroid, direction, random.NextSingle(5f, 20f))
				);

				backgroundItems.Add(asteroid);
			}
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in GetGameObjects()) {
				gameObject.Update(gameTime);
			}
		}

		public IEnumerable<IGameObject> GetGameObjects()
		{
			for (int i = 0; i < backgroundItems.Count; ++i) {
				yield return backgroundItems[i];
			}

			for (int i = 0; i < asteroids.Count; ++i) {
				yield return asteroids[i];
			}

			yield return spaceship;
		}
	}
}
