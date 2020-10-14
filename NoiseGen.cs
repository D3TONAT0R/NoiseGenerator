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
			gen.fractalIterations.value = fractalIterations;
			gen.fractalPersistence = fractalPersistence;
			return gen.Generate();
		}

		public static float[,] GenerateVoronoi(int sizeX, int sizeY, int numOfPeaks, VoronoiGenerator.VoronoiType type) {
			var gen = new VoronoiGenerator(sizeX, sizeY, numOfPeaks, type, 0);
			return gen.Generate();
		}

		public static float[,] GenerateDrunkardLines(int sizeX, int sizeY, int numOfLines, DrunkardLinesGenerator.Mode mode) {
			var gen = new DrunkardLinesGenerator(sizeX, sizeY, numOfLines);
			gen.mode = mode;
			return gen.Generate();
		}
	}
}
