using System;
using Microsoft.Xna.Framework;

namespace Core
{
	public interface IGameObject : IDisposable
	{
		Vector2 Position { get; set; }
		public float Rotation { get; set; }
		float Scale { get; set; }

		void Update(GameTime gameTime);

		void AddComponent(IComponent component);
		TComponent GetComponent<TComponent>() where TComponent : IComponent;
		bool TryGetComponent<TComponent>(out TComponent component) where TComponent : IComponent;
	}
}
