using NoiseGenerator.Utils;
using System;

namespace NoiseGenerator {
	public abstract class AbstractGenerator {

		public RangedInt textureSizeX = new RangedInt(128, 1, 8192);
		public RangedInt textureSizeY = new RangedInt(128, 1, 8192);

		public AbstractGenerator(int sizeX, int sizeY) {
			textureSizeX.Value = sizeX;
			textureSizeY.Value = sizeY;
		}

		public abstract float[,] GenerateNoiseMap();
	}
}
