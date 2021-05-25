using NoiseGenerator.Utils;
using System;
using System.Numerics;

namespace NoiseGenerator
{
	public class PerlinGenerator : AbstractGenerator
	{

		public float offsetX;
		public float offsetY;
		public RangedFloat scale = new RangedFloat(1f, 0.001f, 1000f);
		public RangedInt fractalIterations = new RangedInt(1, 1, 16);
		public float fractalPersistence = -1;
		public float fractalScale = 2f;

		public PerlinGenerator(float pixelsPerCell) : this(0, 0, pixelsPerCell, -1) { }

		public PerlinGenerator(int sizeX, int sizeY, float pixelsPerCell) : this(sizeX, sizeY, pixelsPerCell, -1) { }

		public PerlinGenerator(int sizeX, int sizeY, float pixelsPerCell, int seed) : base(sizeX, sizeY)
		{
			scale.Value = Math.Max(1, pixelsPerCell);
			if (seed == -1)
			{
				seed = new Random().Next(1000000);
			}
			if (seed != 0)
			{
				Random r = new Random(seed);
				offsetX = (float)r.NextDouble() * 65535f;
				offsetY = (float)r.NextDouble() * 65535f;
			}
		}

		public override float[,] GenerateNoiseMap()
		{
			float[,] tex = new float[textureSizeX, textureSizeY];
			for (int x = 0; x < textureSizeX; x++)
			{
				for (int y = 0; y < textureSizeY; y++)
				{
					tex[x, y] = GetPerlinAtCoord(x, y);
				}
			}
			return tex;
		}

		static float DotGrid(int cx, int cy, float x, float y)
		{
			var vec = GetRandomDirectionVector(cx, cy);
			var local = new Vector2(x - cx, y - cy);
			return Vector2.Dot(local, vec);
		}

		static float Lerp(float a, float b, float t)
		{
			return (float)Math.Pow(t, 2f) * (3f - 2f * t) * (b - a) + a;
		}

		public float GetPerlinAtCoord(float x, float y)
		{
			if (fractalPersistence < 0) fractalPersistence = 0.5f;

			float v = 0;
			float s = scale.Value;
			float intensity = 1f;

			for (int i = 0; i < fractalIterations; i++)
			{
				float perlin = CalcPerlin(offsetX + x / s, offsetY + y / s);
				if (i == 0)
				{
					v = perlin;
				}
				else
				{
					v += perlin * fractalPersistence;
				}
				s /= fractalScale;
				intensity *= fractalPersistence;
				if (scale <= 1) break; //We've reached the minimum scale of 1 cell per pixel, there is no need to continue
			}

			return DataConverter.EqualizePerlin(v);
		}

		float CalcPerlin(float x, float y)
		{
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

		static Vector2 GetRandomDirectionVector(int ix, int iy)
		{
			float random = (float)(2920f * Math.Sin(ix * 21942f + iy * 171324f + 8912f) * Math.Cos(ix * 23157f * iy * 217832f + 9758f));
			var vec = new Vector2((float)Math.Sin(random), (float)Math.Cos(random));
			return Vector2.Normalize(vec);
		}
	}
}
