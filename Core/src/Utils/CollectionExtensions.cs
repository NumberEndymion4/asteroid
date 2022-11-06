using System.Collections.Generic;

namespace Core.Utils
{
	public static class CollectionExtensions
	{
		public static bool TryAppendValueSetOrCreate<TKey, TValue, TSet>(
			this IDictionary<TKey, TSet> source, TKey key, TValue value
		) where TSet : ISet<TValue>, new()
		{
			if (source.TryGetValue(key, out TSet set)) {
				return set.Add(value);
			}
			source.Add(key, new TSet { value });
			return true;
		}

		public static bool TryRemoveFromValueCollection<TKey, TValue, TCollection>(
			this IDictionary<TKey, TCollection> source,
			TKey key,
			TValue value,
			bool keepEmpty = true
		) where TCollection : ICollection<TValue>
		{
			if (!source.TryGetValue(key, out var collection) || !collection.Remove(value)) {
				return false;
			}

			if (!keepEmpty && collection.Count == 0) {
				source.Remove(key);
			}
			return true;
		}
	}
}
