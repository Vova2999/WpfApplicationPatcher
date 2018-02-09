using System;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class CommonPropertyTypeExtensions {
		public static bool Is(this CommonProperty assemblyPropertyType, Type propertyType) {
			return propertyType.IsAssignableFrom(assemblyPropertyType.ReflectionProperty.PropertyType.Instance);
		}

		public static bool IsNot(this CommonProperty assemblyPropertyType, Type propertyType) {
			return !assemblyPropertyType.Is(propertyType);
		}
	}
}