namespace Core
{
	public interface ICompletable : IComponent
	{
		bool IsComplete { get; }
	}
}
