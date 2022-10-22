namespace Asteroids.Core
{
	public interface IEnvironment
	{
		IKeyStateProvider GetKeyStateProvider();

		IPresenter GetSpaceshipPresenter(IGameObject spaceship);
		IPresenter GetAsteroidPresenter(IGameObject asteroid);
	}
}
