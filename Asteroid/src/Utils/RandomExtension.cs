﻿using System;

namespace Asteroids.Utils
{
	public static class RandomExtension
	{
		public static float NextSingle(this Random random, float minInclusive, float maxExclusive)
		{
			return minInclusive + (maxExclusive - minInclusive) * random.NextSingle();
		}
	}
}