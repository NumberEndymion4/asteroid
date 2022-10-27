using Microsoft.Xna.Framework;

namespace Core
{
	public interface IGameObject
	{
		Vector2 Position { get; set; }
		public float Rotation { get; set; }
		float Scale { get; set; }

		void Update(GameTime gameTime);

		TBehavior GetBehavior<TBehavior>() where TBehavior : IBehavior;
		bool TryGetBehavior<TBehavior>(out TBehavior behavior) where TBehavior : IBehavior;
	}
}
