﻿using System;
using System.Collections.Generic;

namespace WpfApplicationPatcher.Extensions {
	public static class EnumerableExtensions {
		public static void ForEach<TValue>(this IEnumerable<TValue> values, Action<TValue> actionOnValue) {
			foreach (var value in values)
				actionOnValue(value);
		}
	}
}