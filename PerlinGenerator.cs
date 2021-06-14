using NoiseGenerator.Utils;
using System;
using System.Numerics;

namespace NoiseGenerator
{
	public class PerlinGenerator : Generator
	{

		public float offsetX;
		public float offsetY;
		public float offsetZ;
		public Vector3 scale;
		public RangedInt fractalIterations = new RangedInt(1, 1, 16);
		public float fractalPersistence = -1;
		public float fractalScale = 2f;

		public PerlinGenerator(float scale) : this(new Vector3(scale, scale, scale), -1) { }

		public PerlinGenerator(Vector3 scale, int seed)
		{
			this.scale = scale;
			if (seed == -1)
			{
				seed = new Random().Next(1000000);
			}
			if (seed != 0)
			{
				Random r = new Random(seed);
				offsetX = (float)r.NextDouble() * 65535f;
				offsetY = (float)r.NextDouble() * 65535f;
				offsetZ = (float)r.NextDouble() * 65535f;
			}
		}

		protected override void GenerateNoiseMap(float[,] map)
		{
			for (int x = 0; x < map.GetLength(0); x++)
			{
				for (int y = 0; y < map.GetLength(1); y++)
				{
					map[x, y] = GetPerlinAtCoord(x, y);
				}
			}
		}

		public float[,,] Generate(int width, int height, int depth)
		{
			float[,,] map = new float[width, height, depth];
			for (int x = 0; x < map.GetLength(0); x++)
			{
				for (int y = 0; y < map.GetLength(1); y++)
				{
					for (int z = 0; z < map.GetLength(2); z++)
					{
						map[x, y, z] = GetPerlinAtCoord(x, y, z);
					}
				}
			}
			return map;
		}

		static float DotGrid(int cx, int cy, float x, float y)
		{
			var vec = GetRandomDirectionVector(cx, cy);
			var local = new Vector2(x - cx, y - cy);
			return Vector2.Dot(local, vec);
		}

		static float DotGrid(int cx, int cy, int cz, float x, float y, float z)
		{
			var vec = GetRandomDirectionVector(cx, cy, cz);
			var local = new Vector3(x - cx, y - cy, z - cz);
			return Vector3.Dot(local, vec);
		}

		static float Lerp(float a, float b, float t)
		{
			return (float)Math.Pow(t, 2f) * (3f - 2f * t) * (b - a) + a;
		}

		public float GetPerlinAtCoord(params float[] coords)
		{
			if (coords.Length < 2 || coords.Length > 3)
			{
				throw new ArgumentException("Only 2- or 3-dimensional perlin generation is currently supported");
			}

			if (fractalPersistence < 0) fractalPersistence = 0.5f;

			float v = 0;
			float intensity = 1f;

			Vector3 s = scale;

			for (int i = 0; i < fractalIterations; i++)
			{
				var z = coords.Length > 2 ? coords[2] : 0f;
				Vector3 pos = new Vector3(offsetX + coords[0] * s.X, offsetY + coords[1] * s.Y, offsetZ + z * s.Z);
				float perlin;
				if (coords.Length == 2)
				{
					perlin = CalcPerlin(pos.X, pos.Y);
				}
				else
				{
					perlin = CalcPerlin(pos.X, pos.Y, pos.Z);
				}
				if (i == 0)
				{
					v = perlin;
				}
				else
				{
					v += perlin * fractalPersistence;
				}
				s *= fractalScale;
				intensity *= fractalPersistence;
				//if (scale <= 1) break; //We've reached the minimum scale of 1 cell per pixel, there is no need to continue
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

		float CalcPerlin(float x, float y, float z)
		{
			int x1 = (int)Math.Floor(x);
			int x2 = x1 + 1;
			int y1 = (int)Math.Floor(y);
			int y2 = y1 + 1;
			int z1 = (int)Math.Floor(z);
			int z2 = z1 + 1;

			//Interpolation weights
			float wx = x - x1;
			float wy = y - y1;
			float wz = z - z1;

			//Interpolate on the x axis
			float ix1 = Lerp(DotGrid(x1, y1, z1, x, y, z), DotGrid(x2, y1, z1, x, y, z), wx);
			float ix2 = Lerp(DotGrid(x1, y2, z1, x, y, z), DotGrid(x2, y2, z1, x, y, z), wx);
			float ix3 = Lerp(DotGrid(x1, y1, z2, x, y, z), DotGrid(x2, y1, z2, x, y, z), wx);
			float ix4 = Lerp(DotGrid(x1, y2, z2, x, y, z), DotGrid(x2, y2, z2, x, y, z), wx);

			//Interpolate on the y axis
			float iy1 = Lerp(ix1, ix2, wy);
			float iy2 = Lerp(ix3, ix4, wy);

			//Interpolate on the z axis
			float value = Lerp(iy1, iy2, wz);

			return value;
		}

		static Vector2 GetRandomDirectionVector(int ix, int iy)
		{
			float random = (float)(2920f * Math.Sin(ix * 21942f + iy * 171324f + 8912f) * Math.Cos(ix * 23157f * iy * 217832f + 9758f));
			var vec = new Vector2((float)Math.Sin(random), (float)Math.Cos(random));
			return Vector2.Normalize(vec);
		}

		static Vector3 GetRandomDirectionVector(int ix, int iy, int iz)
		{
			float random = (float)(2920f * Math.Sin(ix * 21942f + iy * 171324f + 8912f) * Math.Cos(ix * 23157f * iy * 217832f + 9758f));
			var vec = new Vector3((float)Math.Sin(random), (float)Math.Cos(random), (float)Math.Sin(random * random * 18.181f + iz*0.11253f));
			return Vector3.Normalize(vec);
		}
	}
}
