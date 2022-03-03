using System.Numerics;

namespace NoiseGenerator
{
	public class NoiseGen
	{

		public static byte[,] CreateBitmapData(float[,] data)
		{
			return DataConverter.ConvertToBitmapValue(data);
		}

		public static byte[,,] CreateBitmapData(float[,,] data)
		{
			return DataConverter.ConvertToBitmapValue(data);
		}

		public static float[,] GeneratePerlinSimple(int sizeX, int sizeY, float pixelsPerCell)
		{
			return GeneratePerlinFractal(sizeX, sizeY, 1f / pixelsPerCell, 0, 0);
		}

		public static float[,] GeneratePerlinFractal(int sizeX, int sizeY, float pixelsPerCell)
		{
			return GeneratePerlinFractal(sizeX, sizeY, 1f / pixelsPerCell, 8, -1);
		}

		public static float[,] GeneratePerlinFractal(int sizeX, int sizeY, float pixelsPerCell, int fractalIterations, bool equalized = false)
		{
			return GeneratePerlinFractal(sizeX, sizeY, 1f / pixelsPerCell, fractalIterations, -1, equalized);
		}

		public static float[,] GeneratePerlinFractal(int sizeX, int sizeY, float scale, int fractalIterations, float fractalPersistence, bool equalized = false)
		{
			var gen = new PerlinGenerator(scale, equalized);
			gen.fractalIterations.Value = fractalIterations;
			gen.fractalPersistence = fractalPersistence;
			return gen.Generate(sizeX, sizeY);
		}

		public static float[,,] GeneratePerlinFractal3D(int sizeX, int sizeY, int sizeZ, Vector3 scale, int fractalIterations, float fractalPersistence, bool equalized = false)
		{
			var gen = new PerlinGenerator(scale, -1, equalized);
			gen.fractalIterations.Value = fractalIterations;
			gen.fractalPersistence = fractalPersistence;
			var map = gen.Generate(sizeX, sizeY, sizeZ);
			return map;
		}

		public static float[,] GenerateVoronoi(int sizeX, int sizeY, int numOfPeaks, VoronoiGenerator.VoronoiType type)
		{
			var gen = new VoronoiGenerator(numOfPeaks, type, 0);
			return gen.Generate(sizeX, sizeY);
		}

		public static float[,] GenerateDrunkardLines(int sizeX, int sizeY, int numOfLines, DrunkardLinesGenerator.Mode mode)
		{
			var gen = new DrunkardLinesGenerator(sizeX, sizeY, numOfLines);
			gen.mode = mode;
			return gen.Generate(sizeX, sizeY);
		}
	}
}
