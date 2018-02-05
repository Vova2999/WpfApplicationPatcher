using System;
using WpfApplicationPatcher.AssemblyTypes;

namespace WpfApplicationPatcher.Extensions {
	public static class AssemblyPropertyTypeExtensions {
		public static bool Is(this AssemblyPropertyType assemblyPropertyType, Type propertyType) {
			return propertyType.IsAssignableFrom(assemblyPropertyType.ReflectionProperty.PropertyType);
		}

		public static bool IsNot(this AssemblyPropertyType assemblyPropertyType, Type propertyType) {
			return !assemblyPropertyType.Is(propertyType);
		}
	}
}