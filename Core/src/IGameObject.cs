using Microsoft.Xna.Framework;

namespace Core
{
	public interface IGameObject
	{
		Vector2 Position { get; set; }
		public float Rotation { get; set; }
		float Scale { get; set; }

		void Update(GameTime gameTime);

		TComponent GetComponent<TComponent>() where TComponent : IComponent;
		bool TryGetComponent<TComponent>(out TComponent component) where TComponent : IComponent;
	}
}
