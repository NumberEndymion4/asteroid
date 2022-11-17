using System;
using Asteroids;
using Client.Broadcast;
using Client.Components;
using Client.Presenters;
using Core;
using Core.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
	internal partial class GameApp : Game, IGameEnvironment
	{
		private KeyboardBroadcast keyboardBroadcast;
		private GameManager gameManager;
		private SpriteBatch spriteBatch;
		private Texture2D spaceshipTexture;
		private Texture2D asteroidTexture;
		private Texture2D bulletTexture;
		private Texture2D laserTexture;
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
			keyboardBroadcast = new KeyboardBroadcast();
			keyboardBroadcast.RegisterTags(Keys.Up, "start_accelerate", "stop_accelerate");
			keyboardBroadcast.RegisterTags(Keys.Left, "start_rotate_CCW", "stop_rotate_CCW");
			keyboardBroadcast.RegisterTags(Keys.Right, "start_rotate_CW", "stop_rotate_CW");
			keyboardBroadcast.RegisterTags(Keys.Space, "start_shoot1", "stop_shoot1");
			keyboardBroadcast.RegisterTags(Keys.Q, "start_shoot2", "stop_shoot2");

			gameManager = new GameManager(this);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			spaceshipTexture = Content.Load<Texture2D>("spaceship");
			asteroidTexture = Content.Load<Texture2D>("asteroid");
			bulletTexture = Content.Load<Texture2D>("bullet");
			laserTexture = Content.Load<Texture2D>("laser");
			font = Content.Load<SpriteFont>("font");

			LoadPartial();
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			} else {
				keyboardBroadcast.Update(gameTime);
				gameManager.EnsureScene();
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

		IPresenter IGameEnvironment.GetSpaceshipPresenter(IGameObject spaceship)
		{
			const int Radius = 100;

			var bounds = new Rectangle(Point.Zero, new Point(Radius));
			var aliveAnimation = new SpriteAnimation(spaceshipTexture);
			aliveAnimation.AppendRegion(bounds);

			var deadAnimation = new SpriteAnimation(spaceshipTexture);
			for (int i = 0; i < 8; ++i) {
				deadAnimation.AppendRegion(bounds);
				bounds.Offset(Radius, 0);
			}

			var animationProvider = new AnimationProvider();
			animationProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			animationProvider.AddSpriteAnimation(LifeCycleState.Dead.ToString(), deadAnimation);
			spaceship.AddComponent(animationProvider);

			return new GameObjectPresenter(spaceship, animationProvider);
		}

		IPresenter IGameEnvironment.GetAsteroidPresenter(IGameObject asteroid)
		{
			const int Radius = 100;

			var aliveAnimation = new SpriteAnimation(asteroidTexture);
			aliveAnimation.AppendRegion(new Rectangle(new Point(Radius, 0), new Point(Radius)));

			var animationProvider = new AnimationProvider();
			animationProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			asteroid.AddComponent(animationProvider);

			return new GameObjectPresenter(asteroid, animationProvider);
		}

		IPresenter IGameEnvironment.GetBulletPresenter(IGameObject bullet)
		{
			const int Width = 25;
			const int Height = 12;

			var aliveAnimation = new SpriteAnimation(bulletTexture);
			aliveAnimation.AppendRegion(new Rectangle(Point.Zero, new Point(Width, Height)));

			var animationProvider = new AnimationProvider();
			animationProvider.AddSpriteAnimation(LifeCycleState.Alive.ToString(), aliveAnimation);
			bullet.AddComponent(animationProvider);

			return new GameObjectPresenter(bullet, animationProvider);
		}

		IPresenter IGameEnvironment.GetLaserPresenter(IGameObject laser)
		{
			const int Width = 100;
			const int Height = 10;

			var bounds = new Rectangle(Point.Zero, new Point(Width, Height));
			var origin = new Vector2(0, 0.5f);
			var screenSize = Config.Instance.ScreenRect.Size.ToVector2();
			var scale = new Vector2(screenSize.Length() / Width, 1f);
			var deadAnimation = new SpriteAnimation(laserTexture);

			for (int i = 0; i < 6; ++i) {
				deadAnimation.AppendRegion(bounds, origin, scale);
				bounds.Offset(0, Height);
			}

			var animationProvider = new AnimationProvider();
			animationProvider.AddSpriteAnimation(LifeCycleState.Dead.ToString(), deadAnimation);
			laser.AddComponent(animationProvider);

			return new GameObjectPresenter(laser, animationProvider);
		}

		IPresenter IGameEnvironment.GetCircleColliderPresenter(CircleCollider collider)
		{
			IPresenter presenter = null;
			ObtainCircleColliderPresenterPartial(collider, ref presenter);
			return presenter;
		}

		IPresenter IGameEnvironment.GetScoresToHudPresenter(IDataProvider<int> scoresProvider)
		{
			const int Offset = 10;

			return new TextPresenter<int>(
				font, new Vector2(Offset), scoresProvider, ScoresToString
			);

			static string ScoresToString(int scores) => $"Scores: {scores}";
		}

		IPresenter IGameEnvironment.GetSpaceshipPositionToHudPresenter(
			IDataProvider<Vector2> positionProvider
		) {
			const int Offset = 10;
			const int PositionY = 24;

			return new TextPresenter<Vector2>(
				font, new Vector2(Offset, Offset + PositionY), positionProvider, PositionToString
			);

			static string PositionToString(Vector2 position) =>
				$"Coordinates: ({position.X:F0}; {position.Y:F0})";
		}

		IPresenter IGameEnvironment.GetSpaceshipAngleToHudPresenter(
			IDataProvider<float> angleProvider
		) {
			const int Offset = 10;
			const int PositionY = 48;

			return new TextPresenter<float>(
				font, new Vector2(Offset, Offset + PositionY), angleProvider, AngleToString
			);

			static string AngleToString(float angle) => $"Angle: {angle:F0}";
		}

		IPresenter IGameEnvironment.GetSpaceshipSpeedToHudPresenter(
			IDataProvider<float> speedProvider
		) {
			const int Offset = 10;
			const int PositionY = 72;

			return new TextPresenter<float>(
				font, new Vector2(Offset, Offset + PositionY), speedProvider, SpeedToString
			);

			static string SpeedToString(float speed) => $"Speed: {speed:F0}";
		}

		IPresenter IGameEnvironment.GetLaserCooldownToHudPresenter(
			IDataProvider<TimeSpan> cooldownProvider
		) {
			const int Offset = 10;
			const int PositionY = 96;

			return new TextPresenter<TimeSpan>(
				font, new Vector2(Offset, Offset + PositionY), cooldownProvider, CooldownToString
			);

			static string CooldownToString(TimeSpan timeSpan) => timeSpan > TimeSpan.Zero
				? $"Laser: {timeSpan.TotalSeconds:F2} sec"
				: $"Laser READY";
		}

		partial void LoadPartial();

		partial void ObtainCircleColliderPresenterPartial(
			CircleCollider collider, ref IPresenter presenter
		);
	}
}
