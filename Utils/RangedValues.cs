

using System;

namespace NoiseGenerator.Utils {
	public struct RangedFloat {

		public float value {
			get { return v; }
			set {
				v = Math.Max(min, Math.Min(max, value));
			}
		}

		private float v;

		public float min;
		public float max;

		public RangedFloat(float value, float min, float max) {
			v = value;
			this.min = min;
			this.max = max;
		}

		public static implicit operator float(RangedFloat r) {
			return r.value;
		}
	}

	public struct RangedInt {

		public int value {
			get { return v; }
			set {
				v = Math.Max(min, Math.Min(max, value));
			}
		}

		private int v;

		public int min;
		public int max;

		public RangedInt(int value, int min, int max) {
			v = value;
			this.min = min;
			this.max = max;
		}

		public static implicit operator int(RangedInt r) {
			return r.value;
		}
	}
}
