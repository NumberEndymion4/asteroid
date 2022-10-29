using System.Collections.Generic;
using Asteroids;
using Client.Components;
using Client.Presenters;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
	internal class GameApp : Game, IGameEnvironment, IKeyStateProvider
	{
		private readonly GameManager gameManager;
		private readonly Dictionary<IGameObject, IPresenter> presenterCache;

		private SpriteBatch spriteBatch;
		private Texture2D spaceshipTexture;
		private Texture2D asteroidTexture;
		private Texture2D circleTexture;
		private SpriteFont font;

		public GameApp()
		{
			_ = new GraphicsDeviceManager(this) {
				PreferredBackBufferWidth = Config.Instance.WindowWidth,
				PreferredBackBufferHeight = Config.Instance.WindowHeight
			};

			Content.RootDirectory = "data";
			IsMouseVisible = true;

			gameManager = new GameManager();
			presenterCache = new Dictionary<IGameObject, IPresenter>();
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
			gameManager.Render(spriteBatch, gameTime);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		IKeyStateProvider IGameEnvironment.GetKeyStateProvider()
		{
			return this;
		}

		IPresenter IGameEnvironment.GetSpaceshipPresenter(IGameObject spaceship)
		{
			if (presenterCache.TryGetValue(spaceship, out var cachedPresenter)) {
				return cachedPresenter;
			}

			var region = new Rectangle(Point.Zero, new Point(100));
			var aliveAnimation = new SpriteAnimation(spaceshipTexture);
			aliveAnimation.AppendRegion(region);

			var deadAimation = new SpriteAnimation(spaceshipTexture);
			for (int i = 0; i < 8; ++i) {
				deadAimation.AppendRegion(region);
				region.Offset(100, 0);
			}

			if (!spaceship.TryGetComponent(out AnimationIdProvider animationIdProvider)) {
				animationIdProvider = new AnimationIdProvider(spaceship);
				spaceship.AddComponent(animationIdProvider);
			}

			var presenter = new GameObjectPresenter(spaceship, animationIdProvider);
			presenter.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			presenter.AddSpriteAnimation(LifeCycleState.Dead.ToString(), deadAimation);

			presenterCache.Add(spaceship, presenter);
			return presenter;
		}

		IPresenter IGameEnvironment.GetAsteroidPresenter(IGameObject asteroid)
		{
			if (presenterCache.TryGetValue(asteroid, out var cachedPresenter)) {
				return cachedPresenter;
			}

			var aliveAnimation = new SpriteAnimation(asteroidTexture);
			aliveAnimation.AppendRegion(new Rectangle(new Point(100, 0), new Point(100)));

			if (!asteroid.TryGetComponent(out AnimationIdProvider animationIdProvider)) {
				animationIdProvider = new AnimationIdProvider(asteroid);
				asteroid.AddComponent(animationIdProvider);
			}

			var presenter = new GameObjectPresenter(asteroid, animationIdProvider);
			presenter.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);

			presenterCache.Add(asteroid, presenter);
			return presenter;
		}

		IPresenter IGameEnvironment.GetBoundsPresenter(ICollider collider)
		{
			return new ColliderPresenter(collider, circleTexture);
		}

		IPresenter IGameEnvironment.GetSpaceshipPositionToHudPresenter(
			IDataProvider<Vector2> positionProvider
		) {
			return new TextPresenter<Vector2>(
				font, Vector2.Zero, positionProvider, PositionToString
			);

			static string PositionToString(Vector2 position) =>
				$"Coordinates: ({position.X:F0}; {position.Y:F0})";
		}

		IPresenter IGameEnvironment.GetSpaceshipAngleToHudPresenter(IDataProvider<float> angleProvider)
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
