using System;
using System.Collections.Generic;
using System.Text;

namespace NoiseGenerator {
	public class NoiseGen {

		public static byte[,] CreateBitmapData(float[,] data) {
			return DataConverter.ConvertToBitmapValue(data);
		}

		public static float[,] GeneratePerlinSimple(int sizeX, int sizeY, float pixelsPerCell) {
			return GeneratePerlinFractal(sizeX, sizeY, pixelsPerCell, 0, 0);
		}

		public static float[,] GeneratePerlinFractal(int sizeX, int sizeY, float pixelsPerCell) {
			return GeneratePerlinFractal(sizeX, sizeY, pixelsPerCell, 8, -1);
		}

		public static float[,] GeneratePerlinFractal(int sizeX, int sizeY, float pixelsPerCell, int fractalIterations) {
			return GeneratePerlinFractal(sizeX, sizeY, pixelsPerCell, fractalIterations, -1);
		}

		public static float[,] GeneratePerlinFractal(int sizeX, int sizeY, float pixelsPerCell, int fractalIterations, float fractalPersistence) {
			var gen = new PerlinGenerator(sizeX, sizeY, pixelsPerCell);
			gen.fractalIterations = fractalIterations;
			gen.fractalPersistence = fractalPersistence;
			return gen.Generate();
		}
	}
}
