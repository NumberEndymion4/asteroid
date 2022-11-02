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
		private GameManager gameManager;
		private SpriteBatch spriteBatch;
		private Texture2D spaceshipTexture;
		private Texture2D asteroidTexture;
		private Texture2D bulletTexture;
		private SpriteFont font;

		public GameApp()
		{
			_ = new GraphicsDeviceManager(this) {
				PreferredBackBufferWidth = Config.Instance.WindowWidth,
				PreferredBackBufferHeight = Config.Instance.WindowHeight
			};

			Content.RootDirectory = "data";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			gameManager = new GameManager(this);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			spaceshipTexture = Content.Load<Texture2D>("spaceship");
			asteroidTexture = Content.Load<Texture2D>("asteroid");
			bulletTexture = Content.Load<Texture2D>("bullet");
			font = Content.Load<SpriteFont>("font");

			LoadPartial();
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			gameManager.EnsureScene();

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
			var region = new Rectangle(Point.Zero, new Point(100));
			var aliveAnimation = new SpriteAnimation(spaceshipTexture);
			aliveAnimation.AppendRegion(region);

			var deadAimation = new SpriteAnimation(spaceshipTexture);
			for (int i = 0; i < 8; ++i) {
				deadAimation.AppendRegion(region);
				region.Offset(100, 0);
			}

			var animationProvider = new AnimationProvider(spaceship);
			animationProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			animationProvider.AddSpriteAnimation(LifeCycleState.Dead.ToString(), deadAimation);
			spaceship.AddComponent(animationProvider);

			return new GameObjectPresenter(spaceship, animationProvider);
		}

		IPresenter IGameEnvironment.GetAsteroidPresenter(IGameObject asteroid)
		{
			var aliveAnimation = new SpriteAnimation(asteroidTexture);
			aliveAnimation.AppendRegion(new Rectangle(new Point(100, 0), new Point(100)));

			var animationProvider = new AnimationProvider(asteroid);
			animationProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			asteroid.AddComponent(animationProvider);

			return new GameObjectPresenter(asteroid, animationProvider);
		}

		IPresenter IGameEnvironment.GetBulletPresenter(IGameObject bullet)
		{
			var aliveAnimation = new SpriteAnimation(bulletTexture);
			aliveAnimation.AppendRegion(new Rectangle(Point.Zero, new Point(25, 12)));

			var animationProvider = new AnimationProvider(bullet);
			animationProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			bullet.AddComponent(animationProvider);

			return new GameObjectPresenter(bullet, animationProvider);
		}

		IPresenter IGameEnvironment.GetBoundsPresenter(ICollider collider)
		{
			IPresenter presenter = null;
			ObtainBoundsPresenterPartial(collider, ref presenter);
			return presenter;
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

		IPresenter IGameEnvironment.GetSpaceshipAngleToHudPresenter(
			IDataProvider<float> angleProvider
		) {
			return new TextPresenter<float>(
				font, new Vector2(0, 24), angleProvider, AngleToString
			);

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
