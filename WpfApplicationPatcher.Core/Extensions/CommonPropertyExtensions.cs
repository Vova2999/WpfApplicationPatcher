using System;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class CommonPropertyExtensions {
		public static bool Is(this CommonProperty commonProperty, Type type) {
			return type.IsAssignableFrom(commonProperty.ReflectionProperty.PropertyType.Instance);
		}

		public static bool IsNot(this CommonProperty commonProperty, Type type) {
			return !commonProperty.Is(type);
		}
	}
}