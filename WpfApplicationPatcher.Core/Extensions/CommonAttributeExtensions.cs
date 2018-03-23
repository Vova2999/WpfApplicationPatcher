using System;
using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class CommonAttributeExtensions {
		public static bool Contains(this IEnumerable<CommonAttribute> commonAttributes, Type type) {
			return commonAttributes.Any(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute.Instance.GetType() == type);
		}

		public static bool NotContains(this IEnumerable<CommonAttribute> commonAttributes, Type type) {
			return !commonAttributes.Contains(type);
		}
	}
}