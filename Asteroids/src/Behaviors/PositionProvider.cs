using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Behaviors
{
	internal class PositionProvider : Behavior, IDataProvider<Vector2>
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
