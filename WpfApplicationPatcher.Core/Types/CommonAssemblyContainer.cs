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

		public CommonType GetCommonType(Type type) {
			return CommonAssemblyTypes.FirstOrDefault(commonType => commonType.ReflectionType.Instance == type)
				?? throw new ArgumentException($"Not found type '{type.FullName}'");
		}

		public IEnumerable<CommonType> GetInheritanceCommonTypes(Type type) {
			return CommonAssemblyTypes.Where(commonType => commonType.Is(type));
		}
	}
}