using NoiseGenerator.Utils;
using System;

namespace NoiseGenerator
{
	public abstract class Generator
	{
		public float[,] Generate(int width, int height)
		{
			float[,] map = new float[width, height];
			GenerateNoiseMap(map);
			return map;
		}

		protected abstract void GenerateNoiseMap(float[,] map);
	}
}
