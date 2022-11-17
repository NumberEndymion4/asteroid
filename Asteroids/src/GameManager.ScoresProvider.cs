using Core;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	public partial class GameManager
	{
		private class ScoresProvider : IDataProvider<int>
		{
			private readonly GameManager manager;

			int IDataProvider<int>.Data => manager.totalScores;

			public ScoresProvider(GameManager gameManager)
			{
				manager = gameManager;
			}

			void IComponent.Update(IGameObject gameObject, GameTime gameTime)
			{
			}
		}
	}
}
