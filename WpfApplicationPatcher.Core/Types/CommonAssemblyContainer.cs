using System;
using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Types {
	public class CommonAssemblyContainer {
		public readonly CommonType[] CommonAssemblyTypes;

		public CommonAssemblyContainer(CommonType[] commonAssemblyTypes) {
			CommonAssemblyTypes = commonAssemblyTypes;
		}

		public CommonType GetAssemblyTypeByReflectionType(Type reflectionType) {
			return CommonAssemblyTypes.FirstOrDefault(assemblyType => assemblyType.ReflectionType.Instance == reflectionType)
				?? throw new ArgumentException($"Not found type '{reflectionType.FullName}'");
		}

		public IEnumerable<CommonType> GetInheritanceAssemblyTypes(CommonType assemblyType) {
			return GetInheritanceAssemblyTypes(assemblyType.ReflectionType.Instance);
		}

		public IEnumerable<CommonType> GetInheritanceAssemblyTypes(Type reflectionType) {
			return CommonAssemblyTypes.Where(assemblyType => assemblyType.Is(reflectionType));
		}
	}
}