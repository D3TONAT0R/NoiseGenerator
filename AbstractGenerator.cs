using System;
using System.Collections.Generic;
using System.Text;

namespace NoiseGenerator {
	public abstract class AbstractGenerator {

		public int textureSizeX;
		public int textureSizeY;

		public AbstractGenerator(int sizeX, int sizeY) {
			textureSizeX = Math.Max(1, sizeX);
			textureSizeY = Math.Max(1, sizeY);
		}

		public abstract float[,] Generate();
	}
}
