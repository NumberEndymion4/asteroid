using Asteroids.Core;
using Asteroids.GameLayer.Behaviors;

namespace Asteroids.GameLayer
{
	public interface IEnvironment
	{
		IKeyStateProvider GetKeyStateProvider();

		IPresenter GetSpaceshipPresenter(IGameObject spaceship);
		IPresenter GetAsteroidPresenter(IGameObject asteroid);
		IPresenter GetBoundsPresenter(CircleCollider collider);
	}
}
