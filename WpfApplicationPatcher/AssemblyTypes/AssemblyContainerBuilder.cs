using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.Extensions;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class AssemblyContainerBuilder {
		public static AssemblyContainer Build(ReflectionAssembly reflectionAssembly, AssemblyDefinition monoCecilAssembly) {
			var allTypes = GetAllTypes(monoCecilAssembly);
			return CreateAssemblyContainer(reflectionAssembly, allTypes);
		}

		private static IEnumerable<TypeDefinition> GetAllTypes(AssemblyDefinition monoCecilAssembly) {
			var types = new List<TypeDefinition>();
			monoCecilAssembly.MainModule.Types.ForEach(type => AddType(types, type));

			return types;
		}

		private static void AddType(List<TypeDefinition> types, TypeDefinition currentType) {
			while (currentType != null) {
				if (types.Contains(currentType))
					break;

				types.Add(currentType);
				currentType = currentType.BaseType?.Resolve();
			}
		}

		private static AssemblyContainer CreateAssemblyContainer(ReflectionAssembly reflectionAssembly, IEnumerable<TypeDefinition> allTypes) {
			return new AssemblyContainer(allTypes
				.Select(type => new AssemblyType(type.FullName, reflectionAssembly.GetReflectionTypeByName(type.FullName), type))
				.Where(assemblyType => assemblyType.ReflectionType != null)
				.ToArray());
		}
	}
}