using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	public class GameObject : Disposable, IGameObject, IEquatable<IGameObject>
	{
		private readonly List<IComponent> components;

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
				component.Update(this, gameTime);
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

		public bool TryGetComponent<TComponent>(out TComponent component)
			where TComponent : IComponent
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

		public virtual bool Equals(IGameObject other)
		{
			return other?.GetType() == typeof(GameObject)
				? ReferenceEquals(this, other)
				: other?.Equals(this) == true;
		}

		public override bool Equals(object other)
		{
			return Equals(other as IGameObject);
		}

		public override int GetHashCode()
		{
			// No special code needed, just suppress warning CS0659.
			return base.GetHashCode();
		}

		protected override void PerformDispose()
		{
			foreach (var component in components) {
				(component as IDisposable)?.Dispose();
			}
			components.Clear();
			base.PerformDispose();
		}
	}
}
