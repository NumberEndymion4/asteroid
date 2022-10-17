using System.Collections.Generic;
using Asteroids.GameLayer;
using Microsoft.Xna.Framework;

namespace Asteroids.Core
{
	public interface IGameObject
	{
		Vector2 Position { get; set; }
		public float Rotation { get; set; }
		float Scale { get; set; }

		void Update(GameTime gameTime);
	}

	public class GameObject : IGameObject
	{
		public List<IBehavior> Behaviors { get; }
		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }

		public GameObject()
		{
			Behaviors = new List<IBehavior>();
			Scale = 1f;
		}

		public void Update(GameTime gameTime)
		{
			foreach (var behavior in Behaviors) {
				behavior.Update(gameTime);
			}
		}
	}
}
