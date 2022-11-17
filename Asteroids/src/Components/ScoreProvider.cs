using Core;
using Microsoft.Xna.Framework;

namespace Asteroids.Components
{
	internal class ScoreProvider : IDataProvider<int>
	{
		public int Score { get; }

		int IDataProvider<int>.Data => Score;

		public ScoreProvider(int score)
		{
			Score = score;
		}

		public void Update(IGameObject gameObject, GameTime gameTime)
		{
		}
	}
}
