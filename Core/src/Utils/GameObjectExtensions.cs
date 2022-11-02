namespace Core.Utils
{
	public static class GameObjectExtensions
	{
		public static bool IsDead(this IGameObject gameObject)
		{
			var lifeCycleProvider = gameObject.GetComponent<IDataProvider<LifeCycleState>>();
			return lifeCycleProvider?.Data == LifeCycleState.Dead;
		}

		public static bool IsComplete(this IGameObject gameObject)
		{
			return gameObject.GetComponent<ICompletable>()?.IsComplete == true;
		}
	}
}
