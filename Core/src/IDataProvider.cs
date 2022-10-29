namespace Core
{
	public interface IDataProvider<T> : IComponent
	{
		T Data { get; }
	}
}
