using System;
using System.Linq;
using WpfApplicationPatcher.Types.Common;

namespace WpfApplicationPatcher.Extensions {
	public static class AssemblyAttributeTypeExtensions {
		public static bool Contains(this CommonAttribute[] assemblyAttributeTypes, Type attributeType) {
			return assemblyAttributeTypes.Any(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute.GetType() == attributeType);
		}

		public static bool NotContains(this CommonAttribute[] assemblyAttributeTypes, Type attributeType) {
			return !assemblyAttributeTypes.Contains(attributeType);
		}
	}
}