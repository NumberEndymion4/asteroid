namespace Core
{
	public interface ICollider : IComponent
	{
		BoundingCircle Bounds { get; }
	}
}
