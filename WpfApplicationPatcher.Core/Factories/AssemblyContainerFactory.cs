using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Factories {
	public class AssemblyContainerFactory {
		public virtual CommonAssemblyContainer Create(ReflectionAssembly reflectionAssembly, MonoCecilAssembly monoCecilAssembly) {
			var allTypes = GetAllTypes(monoCecilAssembly);
			return CreateAssemblyContainer(reflectionAssembly, allTypes);
		}

		private static IEnumerable<MonoCecilType> GetAllTypes(MonoCecilAssembly monoCecilAssembly) {
			var types = new List<MonoCecilType>();
			monoCecilAssembly.MainModule.Types.ForEach(type => AddType(types, type));

			return types;
		}

		private static void AddType(List<MonoCecilType> types, MonoCecilType currentType) {
			while (currentType != null) {
				if (types.Contains(currentType))
					break;

				types.Add(currentType);
				currentType = currentType.BaseType?.ToMonoCecilType();
			}
		}

		private static CommonAssemblyContainer CreateAssemblyContainer(ReflectionAssembly reflectionAssembly, IEnumerable<MonoCecilType> allTypes) {
			return new CommonAssemblyContainer(allTypes
				.Select(type => new CommonType(type.FullName, type, reflectionAssembly.GetReflectionTypeByName(type.FullName)))
				.Where(assemblyType => assemblyType.ReflectionType != null)
				.OrderBy(assemblyType => assemblyType.FullName.Length)
				.ThenBy(assemblyType => assemblyType.FullName)
				.ToArray());
		}
	}
}