using NoiseGenerator.Utils;
using System;

namespace NoiseGenerator {
	public class DrunkardLinesGenerator : AbstractGenerator {

		public enum Mode {
			Manhattan,
			Freeform,
			CurvedLines
		}

		public RangedInt lines = new RangedInt(0, 1, 100000);
		public RangedFloat complexity = new RangedFloat(0.2f, 0f, 1f);
		public RangedFloat crossOverChance = new RangedFloat(0.5f, 0, 1);
		public int avgLineLength;
		public Mode mode = Mode.CurvedLines;

		private Random random;

		public DrunkardLinesGenerator(int sizeX, int sizeY, int numLines) : base(sizeX, sizeY) {
			lines.value = numLines;
			avgLineLength = (sizeX + sizeY) / 2;
			random = new Random();
		}

		public override float[,] Generate() {
			float[,] tex = new float[textureSizeX, textureSizeY];
			for(int i = 0; i < lines; i++) {
				float value = (float)random.NextDouble() * 0.75f + 0.25f;
				if(mode == Mode.Manhattan) {
					DrunkardFixed(tex, random.Next(textureSizeX), random.Next(textureSizeY), -1, avgLineLength, value);
				} else {
					DrunkardFreeform(tex, (float)random.NextDouble() * textureSizeX, (float)random.NextDouble() * textureSizeY, random.Next(0, 360), avgLineLength, value);
				}
			}
			return tex;
		}

		void DrunkardFixed(float[,] tex, int x, int y, int d, int length, float value) {
			while(length > 0) {
				length--;
				if(x < 0 || y < 0 || x >= textureSizeX || y >= textureSizeX) return;
				if(tex[x, y] == 0) {
					tex[x, y] = value;
				} else {
					if(random.NextDouble() > crossOverChance) tex[x, y] = value;
				}
				if(d < 0 || random.NextDouble() < complexity) d = RandomDir((d + 2) % 4);
				if(d == 0) {
					y++;
				} else if(d == 1) {
					x++;
				} else if(d == 2) {
					y--;
				} else {
					x--;
				}
			}
		}

		void DrunkardFreeform(float[,] tex, float x, float y, float angle, float length, float value) {
			while(length > 0) {
				length--;
				SetValue(tex, x, y, value);
				if(mode == Mode.Freeform) {
					if(random.NextDouble() < complexity) {
						angle += random.Next(-135, 135);
					}
				} else {
					angle += random.Next(-90, 90) * complexity;
				}
				x += (float)Math.Cos(Deg2Rad(angle));
				y += (float)Math.Sin(Deg2Rad(angle));
			}
		}

		float Deg2Rad(float deg) {
			return (float)(deg * Math.PI / 180d);
		}

		void SetValue(float[,] tex, float x, float y, float value) {
			int x1 = (int)x;
			int y1 = (int)y;
			int x2 = x1 + 1;
			int y2 = y1 + 1;
			float wx = x - x1;
			float wy = y - y1;
			SetValueAtIndex(tex, x1, y1, value, 1f-wx);
			SetValueAtIndex(tex, x2, y1, value, 1f-wy);
			SetValueAtIndex(tex, x1, y2, value, wx);
			SetValueAtIndex(tex, x2, y2, value, wy);
		}

		void SetValueAtIndex(float[,] tex, int x, int y, float v, float weight) {
			if(x < 0 || y < 0 || x >= textureSizeX || y >= textureSizeY) return;
			if(weight <= 0) return;
			var old = tex[x, y];
			if(v > old) {
				tex[x, y] = Lerp(old, v, weight);
			}
		}

		float Lerp(float a, float b, float t) {
			return a + (b - a) * t;
		}

		int RandomDir(int exclude) {
			int r = random.Next(4);
			r += exclude + 1;
			return r % 4;
		}
	}
}
