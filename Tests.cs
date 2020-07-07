using System.Collections.Generic;
using NUnit.Framework;

namespace NUnitLoggerSampleProject {
	public class Tests {
		[TestCaseSource(nameof(GetInvalidTestCases))]
		public void InvalidCase((int, int) argument) {
			Assert.Pass();
		}

		static IEnumerable<(int, int)> GetInvalidTestCases() {
			return new[] {
				(0, 1)
			};
		}

		[TestCaseSource(nameof(GetValidTestCases))]
		public void ValidCase(int argument) {
			Assert.Pass();
		}

		static IEnumerable<int> GetValidTestCases() {
			return new[] {
				0
			};
		}
	}
}