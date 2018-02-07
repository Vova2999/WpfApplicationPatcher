using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.Types.Common;
using WpfApplicationPatcher.Types.MonoCecil;
using WpfApplicationPatcher.Types.Reflection;

namespace WpfApplicationPatcher.Extensions {
	public static class AssemblyTypeExtensions {
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