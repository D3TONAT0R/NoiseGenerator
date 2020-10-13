using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;

namespace NoiseGenerator {
	public class PerlinGenerator {

		public float offsetX;
		public float offsetY;
		public int textureSizeX;
		public int textureSizeY;
		public float scale;
		public int fractalIterations = 1;
		public float fractalPersistence = -1;
		public float fractalScale = 2f;

		public PerlinGenerator(int sizeX, int sizeY, float pixelsPerCell) : this(sizeX, sizeY,pixelsPerCell, 0) { }

		public PerlinGenerator(int sizeX, int sizeY, float pixelsPerCell, int seed) {
			textureSizeX = Math.Max(1, sizeX);
			textureSizeY = Math.Max(1, sizeY);
			scale = Math.Max(1, pixelsPerCell);
			if(seed != 0) {
				Random r = new Random();
				offsetX = ((float)r.NextDouble() - 0.5f) * 65535f;
				offsetY = ((float)r.NextDouble() - 0.5f) * 65535f;
			}
		}

		public float[,] Generate() {
			if(fractalPersistence < 0) fractalPersistence = 0.5f;
			float[,] tex = new float[textureSizeX, textureSizeY];
			float intensity = 1f;
			for(int i = 0; i < fractalIterations; i++) {
				for(int x = 0; x < textureSizeX; x++) {
					for(int y = 0; y < textureSizeY; y++) {
						float perlin = GetPerlinAt(x / scale, y / scale);
						if(i == 0) {
							tex[x, y] = perlin;
						} else {
							tex[x, y] += perlin * fractalPersistence;
						}
					}
				}
				scale /= fractalScale;
				intensity *= fractalPersistence;
				if(scale <= 1) break; //We've reached the minimum scale of 1 cell per pixel, there is no need to continue
			}
			//Equalize output to the 0-1 range
			for(int x = 0; x < textureSizeX; x++) {
				for(int y = 0; y < textureSizeY; y++) {
					tex[x, y] = DataConverter.EqualizePerlin(tex[x, y]);
				}
			}
			return tex;
		}

		float DotGrid(int cx, int cy, float x, float y) {
			var vec = GetRandomDirectionVector(cx,cy);
			var local = new Vector2(x - cx, y - cy);
			return Vector2.Dot(local, vec);
		}

		float Lerp(float a, float b, float t) {
			return (float)Math.Pow(t, 2f) * (3f - 2f * t) * (b - a) + a;
		}

		float GetPerlinAt(float x, float y) {
			int x1 = (int)x;
			int y1 = (int)y;
			int x2 = x1 + 1;
			int y2 = y1 + 1;

			//Interpolation weights
			float wx = x - x1;
			float wy = y - y1;

			//Interpolate on the x axis
			float ix1 = Lerp(DotGrid(x1, y1, x, y), DotGrid(x2, y1, x, y), wx);
			float ix2 = Lerp(DotGrid(x1, y2, x, y), DotGrid(x2, y2, x, y), wx);

			//Interpolate on the y axis
			float value = Lerp(ix1, ix2, wy);

			return value;
		}

		Vector2 GetRandomDirectionVector(int ix, int iy) {
			float random = (float)(2920f * Math.Sin(ix * 21942f + iy * 171324f + 8912f) * Math.Cos(ix * 23157f * iy * 217832f + 9758f));
			var vec = new Vector2((float)Math.Sin(random), (float)Math.Cos(random));
			return Vector2.Normalize(vec);
		}
	}
}
