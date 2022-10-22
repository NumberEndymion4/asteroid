using Asteroids.Core;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer.Behaviors
{
	public class PositionProvider : IBehavior, IDataProvider<Vector2>
	{
		public Vector2 Data { get; private set; }

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			Data = gameObject.Position;
		}
	}
}
