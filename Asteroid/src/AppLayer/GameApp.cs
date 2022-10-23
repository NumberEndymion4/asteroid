﻿using Asteroids.AppLayer.Presenters;
using Asteroids.GameLayer;
using Asteroids.GameLayer.Behaviors;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.AppLayer
{
	public class GameApp : Game, IEnvironment, IKeyStateProvider
	{
		private readonly GameManager gameManager;

		private SpriteBatch spriteBatch;
		private Texture2D spaceshipTexture;
		private Texture2D asteroidTexture;
		private Texture2D circleTexture;
		private SpriteFont font;

		public GameApp()
		{
			_ = new GraphicsDeviceManager(this) {
				PreferredBackBufferWidth = Config.Instance.WindowSize.X,
				PreferredBackBufferHeight = Config.Instance.WindowSize.Y
			};

			gameManager = new GameManager(Config.Instance);

			Content.RootDirectory = "data";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			gameManager.Initialize(this);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			spaceshipTexture = Content.Load<Texture2D>("spaceship");
			asteroidTexture = Content.Load<Texture2D>("asteroid");
			circleTexture = Content.Load<Texture2D>("circle");
			font = Content.Load<SpriteFont>("font");

			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.OemTilde)) {
				gameTime.ElapsedGameTime /= 5;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			} else {
				gameManager.Update(gameTime);
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Config.Instance.BackgroundColor);

			spriteBatch.Begin();
			gameManager.Render(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		IKeyStateProvider IEnvironment.GetKeyStateProvider()
		{
			return this;
		}

		IPresenter IEnvironment.GetSpaceshipPresenter(IGameObject spaceship)
		{
			return new GameObjectPresenter(
				spaceship, spaceshipTexture, new Rectangle(Point.Zero, new Point(100))
			);
		}

		IPresenter IEnvironment.GetAsteroidPresenter(IGameObject asteroid)
		{
			return new GameObjectPresenter(
				asteroid, asteroidTexture, new Rectangle(new Point(100, 0), new Point(100))
			);
		}

		IPresenter IEnvironment.GetBoundsPresenter(CircleCollider collider)
		{
			return new ColliderPresenter(collider, circleTexture);
		}

		IPresenter IEnvironment.GetSpaceshipPositionToHudPresenter(
			IDataProvider<Vector2> positionProvider
		) {
			return new TextPresenter<Vector2>(
				font, Vector2.Zero, positionProvider, PositionToString
			);

			static string PositionToString(Vector2 position) =>
				$"Coordinates: ({position.X:F0}; {position.Y:F0})";
		}

		IPresenter IEnvironment.GetSpaceshipAngleToHudPresenter(IDataProvider<float> angleProvider)
		{
			return new TextPresenter<float>(font, new Vector2(0, 24), angleProvider, AngleToString);

			static string AngleToString(float angle) => $"Angle: {angle:F0}";
		}

		bool IKeyStateProvider.IsPressed(Keys keys)
		{
			return Keyboard.GetState().IsKeyDown(keys);
		}
	}
}
