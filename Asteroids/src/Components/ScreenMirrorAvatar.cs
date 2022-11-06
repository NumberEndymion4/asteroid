using System;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal partial class ScreenMirrorAvatar : IEquatable<IGameObject>
	{
		private readonly Vector2 affects;

		private IGameObject underlying;
		private Vector2 offet;

		public ScreenMirrorAvatar(IGameObject source, Orientation orientation)
		{
			underlying = source;

			affects = new Vector2(
				orientation.HasFlag(Orientation.Horizontal) ? 1 : 0,
				orientation.HasFlag(Orientation.Vertical) ? 1 : 0
			);
		}

		public bool Equals(IGameObject other)
		{
			return other?.GetType() == typeof(ScreenMirrorAvatar)
				? (other as ScreenMirrorAvatar).underlying.Equals(underlying)
				: other?.Equals(underlying) == true;
		}

		public override bool Equals(object other)
		{
			return Equals(other as IGameObject);
		}

		public override int GetHashCode()
		{
			return underlying.GetHashCode();
		}

		private void UpdateComponent(IGameObject gameObject, GameTime gameTime)
		{
			var screenRect = Config.Instance.ScreenRect;
			int xSign = underlying.Position.X < screenRect.Center.X ? 1 : -1;
			int ySign = underlying.Position.Y < screenRect.Center.Y ? 1 : -1;

			offet.X = affects.X * xSign * screenRect.Width;
			offet.Y = affects.Y * ySign * screenRect.Height;
		}
	}
}
