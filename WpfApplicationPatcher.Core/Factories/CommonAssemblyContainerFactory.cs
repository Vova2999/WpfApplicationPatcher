using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Factories {
	public class CommonAssemblyContainerFactory {
		public virtual CommonTypeContainer Create(ReflectionAssembly reflectionAssembly, MonoCecilAssembly monoCecilAssembly) {
			var allTypes = GetAllTypes(monoCecilAssembly);
			return CreateCommonAssemblyContainer(reflectionAssembly, allTypes);
		}

		private static IEnumerable<MonoCecilType> GetAllTypes(MonoCecilAssembly monoCecilAssembly) {
			var types = new List<MonoCecilType>();
			monoCecilAssembly.MainModule.Types.ForEach(type => AddType(types, type));

			return types;
		}

		private static void AddType(List<MonoCecilType> types, MonoCecilType currentType) {
			while (currentType != null && !types.Contains(currentType)) {
				types.Add(currentType);
				currentType = currentType.BaseType?.Resolve();
			}
		}

		private static CommonTypeContainer CreateCommonAssemblyContainer(ReflectionAssembly reflectionAssembly, IEnumerable<MonoCecilType> allTypes) {
			return new CommonTypeContainer(allTypes
				.Select(type => new CommonType(type, reflectionAssembly.GetTypeByName(type.FullName)))
				.Where(assemblyType => assemblyType.ReflectionType != null)
				.OrderBy(assemblyType => assemblyType.FullName.Length)
				.ThenBy(assemblyType => assemblyType.FullName)
				.ToArray());
		}
	}
}