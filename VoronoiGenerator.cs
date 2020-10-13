using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NoiseGenerator {
	public class VoronoiGenerator {

		public enum VoronoiType {
			Simple,
			BlackValleys,
			Plateaus
		}

		public int textureSizeX;
		public int textureSizeY;
		public VoronoiType type;
		private Vector2[] peaks;
		private float[] peakHeights;
		private int outlineWidth = 2;

		public VoronoiGenerator(int sizeX, int sizeY, int numPeaks, VoronoiType voronoiType, int seed) {
			textureSizeX = Math.Max(1, sizeX);
			textureSizeY = Math.Max(1, sizeY);
			type = voronoiType;
			Random r = seed == 0 ? new Random() : new Random(seed);
			peaks = new Vector2[numPeaks];
			peakHeights = new float[numPeaks];
			for(int i = 0; i < numPeaks; i++) {
				peaks[i] = new Vector2((float)r.NextDouble(), (float)r.NextDouble());
				peakHeights[i] = 0.5f + (float)r.NextDouble() / 2f;
			}
		}

		public float[,] Generate() {
			float[,] tex = new float[textureSizeX, textureSizeY];
			for(int x = 0; x < textureSizeX; x++) {
				for(int y = 0; y < textureSizeY; y++) {
					float voronoi = GetVoronoiAt(x / (float)textureSizeX, y / (float)textureSizeY);
					tex[x, y] = voronoi;
				}
			}
			if(type == VoronoiType.Plateaus && outlineWidth > 0) {
				var outline = GetOutlineMap(tex);
				for(int x = 0; x < textureSizeX; x++) {
					for(int y = 0; y < textureSizeY; y++) {
						if(outline[x, y]) {
							//Draw black outlines
							tex[x, y] = 0;
						}
					}
				}
			}
			return tex;
		}

		public float GetVoronoiAt(float x, float y) {
			Vector2 pos = new Vector2(x, y);
			if(type == VoronoiType.Simple) {
				float closest = 1f;
				foreach(var pt in peaks) {
					float d = Vector2.Distance(pos, pt);
					if(d < closest) closest = d;
				}
				return 1f - closest;
			} else if(type == VoronoiType.BlackValleys) {
				//BROKEN FOR NOW
				float cd1 = 1f;
				int ci1 = -1;
				float cd2 = 1f;
				int ci2 = -1;
				for(int i = 0; i < peaks.Length; i++) {
					float d = Vector2.Distance(pos, peaks[i]);
					if(d < cd1) {
						cd1 = d;
						ci1 = i;
					}
				}
				for(int i = 0; i < peaks.Length; i++) {
					float d = Vector2.Distance(pos, peaks[i]);
					Vector2 vec = Vector2.Normalize(peaks[ci1] - pos);
					float dot = Vector2.Dot(peaks[i], vec);
					if(dot > 0 && i != ci1 && d < cd2) {
						cd2 = d;
						ci2 = i;
					}
				}
				Vector2 c2 = Vector2.Zero;
				if(ci2 < 0) {
					Vector2[] corners = new Vector2[] { Vector2.Zero, Vector2.One, new Vector2(1, 0), new Vector2(0, 1) };
					float closest = 1f;
					foreach(var c in corners) {
						var d = Vector2.Distance(pos, c);
						if(d < closest) {
							closest = d;
							c2 = c;
						}
					}
				} else {
					c2 = peaks[ci2];
				}
				Vector2 c1 = peaks[ci1];
				float vectorDist = Vector2.Distance(c1, c2);
				float ptDist = Vector2.Distance(pos, c1);
				return 1f - (ptDist / (vectorDist / 2f));
			} else if(type == VoronoiType.Plateaus) {
				float closest = 1f;
				float closestHeight = 0f;
				for(int i = 0; i < peaks.Length; i++) {
					float d = Vector2.Distance(pos, peaks[i]);
					if(d < closest) {
						closest = d;
						closestHeight = peakHeights[i];
					}
				}
				return closestHeight;
			} else {
				return 0;
			}
		}

		private bool[,] GetOutlineMap(float[,] tex) {
			bool[,] map = new bool[textureSizeX, textureSizeY];
			for(int x = 0; x < textureSizeX; x++) {
				for(int y = 0; y < textureSizeY; y++) {
					float target = tex[x, y];
					//Scan all pixels near point
					for(int nx = x - outlineWidth; nx <= x + outlineWidth; nx++) {
						for(int ny = y - outlineWidth; ny <= y + outlineWidth; ny++) {
							if(nx < 0 || ny < 0 || nx >= textureSizeX || ny >= textureSizeY) continue;
							if(tex[nx,ny] != target) {
								map[x, y] = true;
								break;
							}
						}
						if(map[x, y]) break;
					}
				}
			}
			return map;
		}
	}
}
