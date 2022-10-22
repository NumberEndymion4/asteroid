using Asteroids.Core;
using Asteroids.GameLayer;
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

		bool IKeyStateProvider.IsPressed(Keys keys)
		{
			return Keyboard.GetState().IsKeyDown(keys);
		}
	}
}
