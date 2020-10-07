using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Text;

namespace Utilities.SubPrograms {
	public class PerlinGenerator : Utility {

		public int textureSize;
		public float scale;
		public int iterations;

		private Random random;

		public override void Start() {
			random = new Random(12345);
			Console.WriteLine("Size of the texture:");
			textureSize = int.Parse(Console.ReadLine());
			Console.WriteLine("Scale:");
			scale = float.Parse(Console.ReadLine()) * 10f;
			Console.WriteLine("Iterations (fractal noise):");
			iterations = Math.Clamp(int.Parse(Console.ReadLine()), 1, 8);

			float[,] tex = new float[textureSize, textureSize];
			float intensity = 1f;
			for(int i = 0; i < iterations; i++) {
				for(int x = 0; x < textureSize; x++) {
					for(int y = 0; y < textureSize; y++) {
						float rx = (float)x / textureSize;
						float ry = (float)y / textureSize;
						float perlin = GetPerlinAt(rx * scale, ry * scale);
						if(i == 0) {
							tex[x, y] = perlin;
						} else {
							tex[x, y] += perlin * intensity;
						}
					}
				}
				scale *= 2f;
				intensity *= 0.75f;
			}

			Bitmap bmp = new Bitmap(textureSize, textureSize);
			for(int y = 0; y < textureSize; y++) {
				for(int x = 0; x < textureSize; x++) {
					float perlin = (tex[x, y] + 1f)/ 2f;
					int v = Math.Clamp((int)(perlin * 255f), 0, 255);
					Color c = Color.FromArgb(v, v, v);
					bmp.SetPixel(x, y, c);
				}
			}

			int fn = 1;
			string s = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "perlin-");
			while(File.Exists(s+ fn + ".png")) {
				fn++;
			}
			bmp.Save(s + fn + ".png", ImageFormat.Png);
		}

		float DotGrid(int cx, int cy, float x, float y) {
			var vec = GetRandomDirectionVector(cx,cy);
			var local = new Vector2(x - cx, y - cy);
			return Vector2.Dot(local, vec);
		}

		float Lerp(float a, float b, float t) {
			return (float)Math.Pow(t, 2f) * (3f - 2f * t) * (b - a) + a;
			//return a + (b - a) * t;
			//return t >= 0.5f ? b : a;
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
