namespace Asteroids.GameLayer
{
	public interface ICollider
	{
		BoundingCircle Bounds { get; }

		void CollisionWith(ICollider other);
	}
}
