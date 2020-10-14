using System;
using System.Collections.Generic;
using System.Text;

namespace NoiseGenerator.Utils {
	[AttributeUsage(AttributeTargets.Method)]
	public class RangeAttribute : Attribute {

		public int min;
		public int max;

		public RangeAttribute(int min, int max) {
			this.min = min;
			this.max = max;
		}
	}
}
