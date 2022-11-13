using System.Linq;

namespace Core.Utils
{
	public static class GameObjectExtensions
	{
		public static bool IsDead(this IGameObject gameObject)
		{
			return
				gameObject.TryGetComponent(out IDataProvider<LifeCycleState> lifeCycleProvider) &&
				lifeCycleProvider.Data == LifeCycleState.Dead;
		}

		public static bool IsComplete(this IGameObject gameObject)
		{
			return gameObject.EnumerateComponents<ICompletable>()
				.All(completable => completable.IsComplete);
		}
	}
}
