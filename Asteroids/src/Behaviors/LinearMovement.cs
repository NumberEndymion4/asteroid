using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class LinearMovement : Behavior
	{
		private readonly Vector2 direction;
		private readonly float speed;

		public LinearMovement(IGameObject owner, Vector2 moveDirection, float moveSpeed)
			: base(owner)
		{
			direction = moveDirection;
			direction.Normalize();
			speed = moveSpeed;
		}

		public override void Update(GameTime gameTime)
		{
			Owner.Position += direction * speed * gameTime.ElapsedSeconds();
		}
	}
}
