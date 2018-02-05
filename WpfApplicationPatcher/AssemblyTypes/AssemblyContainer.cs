using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class AssemblyContainer {
		public readonly AssemblyType[] AssemblyTypes;

		public AssemblyContainer(AssemblyType[] assemblyTypes) {
			AssemblyTypes = assemblyTypes;
		}

		public AssemblyType GetAssemblyTypeByReflectionType(Type reflectionType) {
			return AssemblyTypes.FirstOrDefault(assemblyType => assemblyType.ReflectionType == reflectionType)
				?? throw new ArgumentException($"Not found type '{reflectionType.FullName}'");
		}

		public IEnumerable<AssemblyType> GetInheritanceAssemblyTypes(Type reflectionType) {
			return AssemblyTypes.Where(assemblyType => reflectionType.IsAssignableFrom(assemblyType.ReflectionType));
		}

		public IEnumerable<AssemblyType> GetInheritanceAssemblyTypes(AssemblyType assemblyType) {
			return GetInheritanceAssemblyTypes(assemblyType.ReflectionType);
		}
	}
}