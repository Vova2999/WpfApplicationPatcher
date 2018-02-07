using System;
using WpfApplicationPatcher.Types.Common;

namespace WpfApplicationPatcher.Extensions {
	public static class AssemblyPropertyTypeExtensions {
		public static bool Is(this CommonProperty assemblyPropertyType, Type propertyType) {
			return propertyType.IsAssignableFrom(assemblyPropertyType.ReflectionProperty.PropertyType);
		}

		public static bool IsNot(this CommonProperty assemblyPropertyType, Type propertyType) {
			return !assemblyPropertyType.Is(propertyType);
		}
	}
}