namespace Core
{
	public interface ICollider
	{
		BoundingCircle Bounds { get; }

		void CollisionWith(ICollider other);
	}
}
