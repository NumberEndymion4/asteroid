using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class PositionProvider : IDataProvider<Vector2>
	{
		public Vector2 Position { get; private set; }

		Vector2 IDataProvider<Vector2>.Data => Position;

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
			Position = gameObject.Position;
		}
	}
}
