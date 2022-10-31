using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class PositionProvider : Component, IDataProvider<Vector2>
	{
		public Vector2 Position { get; private set; }

		Vector2 IDataProvider<Vector2>.Data => Position;

		public PositionProvider(IGameObject owner) : base(owner)
		{
		}

		public override void Update(GameTime gameTime)
		{
			Position = Owner.Position;
		}
	}
}
