using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal partial class ScreenMirrorAvatar : IGameObject, IComponent
	{
		Vector2 IGameObject.Position
		{
			get => underlying.Position + offet;
			set => underlying.Position = value - offet;
		}

		float IGameObject.Rotation
		{
			get => underlying.Rotation;
			set => underlying.Rotation = value;
		}

		float IGameObject.Scale
		{
			get => underlying.Scale;
			set => underlying.Scale = value;
		}

		void IComponent.Update(IGameObject gameObject, GameTime gameTime)
		{
			UpdateComponent(gameObject, gameTime);
		}

		void IGameObject.Update(GameTime gameTime)
		{
		}

		void IGameObject.AddComponent(IComponent component)
		{
			underlying.AddComponent(component);
		}

		TComponent IGameObject.GetComponent<TComponent>()
		{
			return underlying.GetComponent<TComponent>();
		}

		bool IGameObject.TryGetComponent<TComponent>(out TComponent component)
		{
			return underlying.TryGetComponent(out component);
		}

		IEnumerable<TComponent> IGameObject.EnumerateComponents<TComponent>()
		{
			return underlying.EnumerateComponents<TComponent>();
		}

		void IDisposable.Dispose()
		{
			underlying = null;
		}
	}
}
