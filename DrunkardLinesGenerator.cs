using System;
using System.Collections.Generic;
using System.Text;

namespace NoiseGenerator {
	public class DrunkardLinesGenerator : AbstractGenerator {

		public int lines;
		public float complexity = 0.2f;
		public float crossOverChance = 1f;
		public int avgLineLength;

		private Random random;

		public DrunkardLinesGenerator(int sizeX, int sizeY, int numLines) : base(sizeX, sizeY) {
			lines = numLines;
			avgLineLength = (sizeX + sizeY) / 2;
			random = new Random();
		}

		public override float[,] Generate() {
			float[,] tex = new float[textureSizeX, textureSizeY];
			for(int i = 0; i < lines; i++) {
				Drunkard(tex, random.Next(textureSizeX), random.Next(textureSizeY), -1, avgLineLength, (float)random.NextDouble()*0.75f+0.25f);
			}
			return tex;
		}

		void Drunkard(float[,] tex, int x, int y, int d, int length, float value) {
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

		int RandomDir(int exclude) {
			int r = random.Next(4);
			r += exclude + 1;
			return r % 4;
		}
	}
}
