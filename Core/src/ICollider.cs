namespace Core
{
	public interface ICollider : IComponent
	{
		int Group { get; }
		IGameObject Owner { get; }
	}
}
