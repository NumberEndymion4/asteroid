using Asteroids.Core;
using Asteroids.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer.Behaviors
{
	public class LinearMovement : IBehavior
	{
		private readonly IGameObject owner;
		private readonly Vector2 direction;
		private readonly float speed;

		public LinearMovement(IGameObject gameObject, Vector2 moveDirection, float moveSpeed)
		{
			owner = gameObject;
			direction = moveDirection;
			direction.Normalize();
			speed = moveSpeed;
		}

		public void Update(GameTime gameTime)
		{
			owner.Position += direction * speed * gameTime.ElapsedSeconds();
		}
	}
}
