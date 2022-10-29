using System;

namespace Core.Utils
{
	public static class RandomExtension
	{
		public static int NextSign(this Random random)
		{
			return random.Next(2) == 0 ? -1 : 1;
		}

		public static float NextSingle(this Random random, float minInclusive, float maxExclusive)
		{
			return minInclusive + (maxExclusive - minInclusive) * random.NextSingle();
		}
	}
}
