using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	public class GameObject : IGameObject
	{
		private readonly List<IComponent> components;

		private bool isDisposed;

		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }

		public GameObject()
		{
			components = new List<IComponent>();
			Scale = 1f;
		}

		public void Update(GameTime gameTime)
		{
			foreach (var component in components) {
				component.Update(gameTime);
			}
		}

		public void AddComponent(IComponent component)
		{
			components.Add(component);
		}

		public TComponent GetComponent<TComponent>() where TComponent : IComponent
		{
			return TryGetComponent(out TComponent component) ? component : default;
		}

		public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : IComponent
		{
			foreach (var item in components) {
				if (item is TComponent found) {
					component = found;
					return true;
				}
			}

			component = default;
			return false;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (isDisposed) {
				return;
			}

			if (disposing) {
				PerformDispose();
				foreach (var component in components) {
					component.Dispose();
				}
				components.Clear();
			}
			isDisposed = true;
		}

		protected virtual void PerformDispose()
		{
		}
	}
}
