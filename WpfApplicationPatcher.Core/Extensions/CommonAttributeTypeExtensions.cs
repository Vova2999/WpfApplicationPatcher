using System;
using System.Linq;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class CommonAttributeTypeExtensions {
		public static bool Contains(this CommonAttribute[] assemblyAttributeTypes, Type attributeType) {
			return assemblyAttributeTypes.Any(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute.GetType() == attributeType);
		}

		public static bool NotContains(this CommonAttribute[] assemblyAttributeTypes, Type attributeType) {
			return !assemblyAttributeTypes.Contains(attributeType);
		}
	}
}