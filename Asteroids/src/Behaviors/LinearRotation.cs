using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class LinearRotation : Behavior
	{
		private readonly float rps;

		public LinearRotation(IGameObject owner, float radianPerSecond) : base(owner)
		{
			rps = radianPerSecond;
		}

		public override void Update(GameTime gameTime)
		{
			Owner.Rotation += rps * gameTime.ElapsedSeconds();
		}
	}
}
