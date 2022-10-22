using Asteroids.Core;
using Asteroids.GameLayer.Behaviors;
using Microsoft.Xna.Framework;

namespace Asteroids.GameLayer
{
	public interface IEnvironment
	{
		IKeyStateProvider GetKeyStateProvider();

		IPresenter GetSpaceshipPresenter(IGameObject spaceship);
		IPresenter GetAsteroidPresenter(IGameObject asteroid);
		IPresenter GetBoundsPresenter(CircleCollider collider);

		IPresenter GetSpaceshipPositionToHudPresenter(IDataProvider<Vector2> positionProvider);
		IPresenter GetSpaceshipAngleToHudPresenter(IDataProvider<float> angleProvider);
	}
}
