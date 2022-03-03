using System;

namespace NoiseGenerator
{
	internal static class MathUtils
	{

		public static int Clamp(int v, int min, int max)
		{
			return Math.Max(Math.Min(v, max), min);
		}

		public static float Clamp(float v, float min, float max)
		{
			return Math.Max(Math.Min(v, max), min);
		}

		public static float Clamp01(float v)
		{
			return Clamp(v, 0, 1);
		}

		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		public static float InverseLerp(float a, float b, float v)
		{
			return (v - a) / (b - a);
		}

		public static float Remap(float value, float oldA, float oldB, float newA, float newB)
		{
			float t = InverseLerp(oldA, oldB, value);
			return Lerp(newA, newB, t);
		}

		public static float AdvancedSmoothStep(float value, float steepness, float middle = 0.5f)
		{
			float c = 2f / (1f - steepness) - 1f;
			value = Clamp01(value);

			double F(float x, float n)
			{
				return Math.Pow(x, c) / Math.Pow(n, c - 1f);
			}

			if(value < middle)
			{
				return (float)F(value, middle);
			}
			else
			{
				return (float)(1f - F(1f - value, 1f - middle));
			}
		}
	}
}
