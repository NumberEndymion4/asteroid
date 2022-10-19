namespace Asteroids.Core
{
	public interface IEnvironment
	{
		IKeyStateProvider GetKeyStateProvider();

		IPresenter GetSpaceshipPresenter();
		IPresenter GetAsteroidPresenter();
	}
}
