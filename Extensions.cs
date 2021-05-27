using System;
using System.Collections.Generic;
using System.Text;

namespace NoiseGenerator
{
	internal static class Extensions
	{
		public static int Width<T>(this T[,] array)
		{
			return array.GetLength(0);
		}

		public static int Height<T>(this T[,] array)
		{
			return array.GetLength(1);
		}
	}
}
