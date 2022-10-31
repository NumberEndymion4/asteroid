using System;
using Microsoft.Xna.Framework;

namespace Core
{
	public abstract class Component : IComponent
	{
		private bool isDisposed;

		public IGameObject Owner { get; private set; }

		public Component(IGameObject owner)
		{
			Owner = owner;
		}

		public abstract void Update(GameTime gameTime);

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!isDisposed && disposing) {
				PerformDispose();
				Owner = null;
			}
			isDisposed = true;
		}

		protected virtual void PerformDispose()
		{
		}
	}
}
