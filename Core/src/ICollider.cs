namespace Core
{
	public interface ICollider : IBehavior
	{
		BoundingCircle Bounds { get; }
	}
}
