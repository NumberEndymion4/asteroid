using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class InputRotation : Disposable, IComponent, IBroadcastListener
	{
		private readonly float rps;

		private bool isRotateCCW;
		private bool isRotateCW;

		public InputRotation(float radianPerSecond)
		{
			rps = radianPerSecond;
			BroadcastService.Instance.Register(this);
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			if (isRotateCW != isRotateCCW) {
				float radians = (isRotateCW ? rps : -rps) * gameTime.ElapsedSeconds();
				gameObject.Rotation = MathHelper.WrapAngle(gameObject.Rotation + radians);
			}
		}

		public void Notify(IBroadcastMessage message, GameTime gameTime)
		{
			switch (message.Tag) {
				case "start_rotate_CCW": isRotateCCW = true; break;
				case "stop_rotate_CCW": isRotateCCW = false; break;
				case "start_rotate_CW": isRotateCW = true; break;
				case "stop_rotate_CW": isRotateCW = false; break;
			}
		}

		protected override void PerformDispose()
		{
			BroadcastService.Instance.Unregister(this);
		}
	}
}
