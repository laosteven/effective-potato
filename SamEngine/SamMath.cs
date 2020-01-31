using System;

namespace SamEngine
{
	public static class SamMath
	{
		public const float Deg2Rad = 0.0174532924f;

		public const float Rad2Deg = 57.2957764f;

		public static Random Rand;

		static SamMath()
		{
			SamMath.Rand = new Random();
		}

		public static float Clamp(float a, float min, float max)
		{
			return Math.Min(Math.Max(a, min), max);
		}

		public static float Lerp(float a, float b, float p)
		{
			return a * (1f - p) + b * p;
		}

		public static float RandomRange(float min, float max)
		{
			return min + (float)SamMath.Rand.NextDouble() * (max - min);
		}
	}
}