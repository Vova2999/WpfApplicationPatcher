using System;
using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Types {
	public class CommonAssemblyContainer {
		public IEnumerable<string> FullNames => commonAssemblyTypes?.Select(assemblyType => assemblyType.FullName) ?? Enumerable.Empty<string>();
		private readonly CommonType[] commonAssemblyTypes;

		internal CommonAssemblyContainer(CommonType[] commonAssemblyTypes) {
			this.commonAssemblyTypes = commonAssemblyTypes;
		}

		public CommonType GetCommonType(Type type) {
			return commonAssemblyTypes?.FirstOrDefault(commonType => commonType.ReflectionType.Instance == type)
				?? throw new ArgumentException($"Not found type '{type.FullName}'");
		}

		public IEnumerable<CommonType> GetInheritanceCommonTypes(Type type) {
			return commonAssemblyTypes?.Where(commonType => commonType.Is(type));
		}
	}
}