using NoiseGenerator.Utils;
using System;

namespace NoiseGenerator {
	public abstract class AbstractGenerator {

		public RangedInt textureSizeX = new RangedInt(128, 1, 8192);
		public RangedInt textureSizeY = new RangedInt(128, 1, 8192);

		public AbstractGenerator(int sizeX, int sizeY) {
			textureSizeX.value = sizeX;
			textureSizeY.value = sizeY;
		}

		public abstract float[,] Generate();
	}
}
