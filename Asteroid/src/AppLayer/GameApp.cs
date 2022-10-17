using Asteroids.Core;
using Asteroids.GameLayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.AppLayer
{
	public class GameApp : Game
	{
		private class KeyStateProvider : IKeyStateProvider
		{
			public bool IsPressed(Keys keys)
			{
				return Keyboard.GetState().IsKeyDown(keys);
			}
		}

		private readonly GraphicsDeviceManager graphics;
		private readonly IKeyStateProvider keyStateProvider;
		private readonly GameManager gameManager;

		private SpriteBatch spriteBatch;
		private Texture2D spaceshipTexture;
		private Texture2D asteroidTexture;

		public GameApp()
		{
			graphics = new GraphicsDeviceManager(this) {
				PreferredBackBufferWidth = Config.Instance.WindowWidth,
				PreferredBackBufferHeight = Config.Instance.WindowHeight
			};

			keyStateProvider = new KeyStateProvider();
			gameManager = new GameManager(Config.Instance, keyStateProvider);

			Content.RootDirectory = "data";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			gameManager.Initialize();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			spaceshipTexture = Content.Load<Texture2D>("spaceship");
			asteroidTexture = Content.Load<Texture2D>("asteroid");
		}

		protected override void Update(GameTime gameTime)
		{
			if (keyStateProvider.IsPressed(Keys.Escape)) {
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
			foreach (var item in gameManager.GetGameObjects()) {
				if (item is Spaceship) {
					spriteBatch.Draw(
						spaceshipTexture, item.Position, new Rectangle(0, 0, 100, 100), Color.White,
						item.Rotation, Vector2.One * 50f, item.Scale, SpriteEffects.None, 0.0f
					);
				} else {
					spriteBatch.Draw(
						asteroidTexture, item.Position, new Rectangle(100, 0, 100, 100), Color.White,
						item.Rotation, Vector2.One * 50f, item.Scale, SpriteEffects.None, 0.0f
					);
				}
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
