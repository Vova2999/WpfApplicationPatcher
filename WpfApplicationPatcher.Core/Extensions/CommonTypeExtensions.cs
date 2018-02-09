using System;
using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class CommonTypeExtensions {
		public static bool Is(this CommonType assemblyType, Type reflectionType) {
			return reflectionType.IsAssignableFrom(assemblyType.ReflectionType.Instance);
		}

		public static bool IsNot(this CommonType assemblyType, Type reflectionType) {
			return !assemblyType.Is(reflectionType);
		}

		public static IEnumerable<CommonType> WhereFrom(this IEnumerable<CommonType> assemblyTypes, MonoCecilModule module) {
			return assemblyTypes.Where(assemblyType => assemblyType.MonoCecilType.Module == module);
		}
	}
}