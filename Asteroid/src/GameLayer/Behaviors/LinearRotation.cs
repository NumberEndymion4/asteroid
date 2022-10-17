using Asteroids.Core;
using Asteroids.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer.Behaviors
{
	public class LinearRotation : IBehavior
	{
		private readonly IGameObject owner;
		private readonly float rps;

		public LinearRotation(IGameObject gameObject, float radianPerSecond)
		{
			owner = gameObject;
			rps = radianPerSecond;
		}

		public void Update(GameTime gameTime)
		{
			owner.Rotation += rps * gameTime.ElapsedSeconds();
		}
	}
}
