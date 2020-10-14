using System;

namespace NoiseGenerator {
	public static class DataConverter {

		public static byte[,] ConvertToBitmapValue(float[,] array) {
			byte[,] bitmap = new byte[array.GetLength(0), array.GetLength(1)];
			for(int x = 0; x < array.GetLength(0); x++) {
				for(int y = 0; y < array.GetLength(1); y++) {
					bitmap[x, y] = (byte)(Clamp01(array[x, y]) * 255f);
				}
			}
			return bitmap;
		}

		public static byte[,,] ConvertToBitmapValue(float[,,] array) {
			byte[,,] bitmap = new byte[array.GetLength(0), array.GetLength(1), array.GetLength(2)];
			for(int x = 0; x < array.GetLength(0); x++) {
				for(int y = 0; y < array.GetLength(1); y++) {
					for(int z = 0; z < array.GetLength(2); z++) {
						bitmap[x, y, z] = (byte)(Clamp01(array[x, y, z]) * 255f);
					}
				}
			}
			return bitmap;
		}

		public static float EqualizePerlin(float value) {
			return Clamp01((value + 1f) / 2f);
		}

		public static float Clamp01(float value) {
			return Math.Max(0, Math.Min(1, value));
		}
	}
}
