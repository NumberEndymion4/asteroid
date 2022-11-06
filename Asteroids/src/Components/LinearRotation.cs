using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class LinearRotation : IComponent
	{
		private readonly float rps;

		public LinearRotation(float radianPerSecond)
		{
			rps = radianPerSecond;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			gameObject.Rotation += rps * gameTime.ElapsedSeconds();
		}
	}
}
