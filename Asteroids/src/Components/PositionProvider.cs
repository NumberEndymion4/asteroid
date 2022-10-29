using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class PositionProvider : Component, IDataProvider<Vector2>
	{
		public Vector2 Data { get; private set; }

		public PositionProvider(IGameObject owner) : base(owner)
		{
		}

		public override void Update(GameTime gameTime)
		{
			Data = Owner.Position;
		}
	}
}
