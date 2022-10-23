using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class PositionProvider : IBehavior, IDataProvider<Vector2>
	{
		public Vector2 Data { get; private set; }

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			Data = gameObject.Position;
		}
	}
}
