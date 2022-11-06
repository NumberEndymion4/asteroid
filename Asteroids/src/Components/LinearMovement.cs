using Core;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class LinearMovement : IComponent
	{
		private readonly Vector2 direction;
		private readonly float speed;

		public LinearMovement(Vector2 moveDirection, float moveSpeed)
		{
			direction = moveDirection;
			direction.Normalize();
			speed = moveSpeed;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			gameObject.Position += direction * speed * gameTime.ElapsedSeconds();
		}
	}
}
