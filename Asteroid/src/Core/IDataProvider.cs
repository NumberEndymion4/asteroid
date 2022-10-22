namespace Asteroids.Core
{
	public interface IDataProvider<T>
	{
		T Data { get; }
	}
}
