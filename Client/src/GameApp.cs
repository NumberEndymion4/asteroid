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
	internal partial class GameApp : Game, IGameEnvironment, IKeyStateProvider
	{
		private readonly GameManager gameManager;
		private readonly Dictionary<object, IPresenter> presenterCache;

		private SpriteBatch spriteBatch;
		private Texture2D spaceshipTexture;
		private Texture2D asteroidTexture;
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
			presenterCache = new Dictionary<object, IPresenter>();
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
			font = Content.Load<SpriteFont>("font");

			LoadPartial();
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

			var animationIdProvider = new AnimationProvider(spaceship);
			animationIdProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			animationIdProvider.AddSpriteAnimation(LifeCycleState.Dead.ToString(), deadAimation);
			spaceship.AddComponent(animationIdProvider);

			var presenter = new GameObjectPresenter(spaceship, animationIdProvider);
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

			var animationIdProvider = new AnimationProvider(asteroid);
			animationIdProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			asteroid.AddComponent(animationIdProvider);

			var presenter = new GameObjectPresenter(asteroid, animationIdProvider);
			presenterCache.Add(asteroid, presenter);
			return presenter;
		}

		IPresenter IGameEnvironment.GetBoundsPresenter(ICollider collider)
		{
			if (presenterCache.TryGetValue(collider, out var presenter)) {
				return presenter;
			}

			ObtainBoundsPresenterPartial(collider, ref presenter);
			presenterCache.Add(collider, presenter);
			return presenter;
		}

		IPresenter IGameEnvironment.GetSpaceshipPositionToHudPresenter(
			IDataProvider<Vector2> positionProvider
		) {
			if (presenterCache.TryGetValue(positionProvider, out var presenter)) {
				return presenter;
			}

			presenter = new TextPresenter<Vector2>(
				font, Vector2.Zero, positionProvider, PositionToString
			);

			presenterCache.Add(positionProvider, presenter);
			return presenter;

			static string PositionToString(Vector2 position) =>
				$"Coordinates: ({position.X:F0}; {position.Y:F0})";
		}

		IPresenter IGameEnvironment.GetSpaceshipAngleToHudPresenter(
			IDataProvider<float> angleProvider
		) {
			if (presenterCache.TryGetValue(angleProvider, out var presenter)) {
				return presenter;
			}

			presenter = new TextPresenter<float>(
				font, new Vector2(0, 24), angleProvider, AngleToString
			);

			presenterCache.Add(angleProvider, presenter);
			return presenter;

			static string AngleToString(float angle) => $"Angle: {angle:F0}";
		}

		bool IKeyStateProvider.IsPressed(Keys keys)
		{
			return Keyboard.GetState().IsKeyDown(keys);
		}

		partial void LoadPartial();
		partial void ObtainBoundsPresenterPartial(ICollider collider, ref IPresenter presenter);
	}
}
