namespace Core
{
	public interface ICollider : IComponent
	{
		int Group { get; }
		BoundingCircle Bounds { get; }
	}
}
