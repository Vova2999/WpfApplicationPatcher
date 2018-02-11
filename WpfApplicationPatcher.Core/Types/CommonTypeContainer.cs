using System;
using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Types {
	public class CommonTypeContainer {
		public IEnumerable<string> FullNames => commonTypes?.Select(assemblyType => assemblyType.FullName) ?? Enumerable.Empty<string>();
		private readonly CommonType[] commonTypes;

		public CommonTypeContainer(CommonType[] commonTypes) {
			this.commonTypes = commonTypes;
		}

		public CommonType GetCommonType(Type type) {
			return commonTypes?.FirstOrDefault(commonType => commonType.ReflectionType.Instance == type)
				?? throw new ArgumentException($"Not found type '{type.FullName}'");
		}

		public IEnumerable<CommonType> GetInheritanceCommonTypes(Type type) {
			return commonTypes?.Where(commonType => commonType.Is(type));
		}
	}
}