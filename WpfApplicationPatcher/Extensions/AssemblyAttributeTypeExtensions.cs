using System;
using System.Linq;
using WpfApplicationPatcher.AssemblyTypes;

namespace WpfApplicationPatcher.Extensions {
	public static class AssemblyAttributeTypeExtensions {
		public static bool Contains(this AssemblyAttributeType[] assemblyAttributeTypes, Type attributeType) {
			return assemblyAttributeTypes.Any(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute.GetType() == attributeType);
		}

		public static bool NotContains(this AssemblyAttributeType[] assemblyAttributeTypes, Type attributeType) {
			return !assemblyAttributeTypes.Contains(attributeType);
		}
	}
}