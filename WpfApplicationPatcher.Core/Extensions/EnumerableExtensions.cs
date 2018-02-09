using System;
using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Helpers;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class EnumerableExtensions {
		[DoNotAddLogOffset]
		public static void ForEach<TValue>(this IEnumerable<TValue> values, Action<TValue> actionOnValue) {
			foreach (var value in values)
				actionOnValue(value);
		}

		public static TValue[] ToCreatedArray<TValue>(this IEnumerable<TValue> values) {
			return values as TValue[] ?? values.ToArray();
		}
	}
}