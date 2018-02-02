using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.TypesTree;

namespace WpfApplicationPatcher.Extensions {
	public static class TypeDefinitionExtensions {
		public static bool Is(this TypeDefinition type, string typeName) {
			return type.FullName == typeName;
		}

		public static bool IsNot(this TypeDefinition type, string typeName) {
			return !type.Is(typeName);
		}

		public static bool Contains(this IEnumerable<TypeDefinition> types, string typeName) {
			return types.Any(type => type.Is(typeName));
		}

		public static bool NotContains(this IEnumerable<TypeDefinition> types, string typeName) {
			return !types.Contains(typeName);
		}

		public static TypeDefinitionsTree ToTree(this IEnumerable<TypeDefinition> types) {
			var tree = new Dictionary<TypeDefinition, List<TypeDefinition>>();
			foreach (var type in types) {
				var baseType = type;
				var inheritanceType = (TypeDefinition)null;
				while (baseType != null) {
					if (tree.TryGetValue(baseType, out var listTypes)) {
						if (listTypes.Contains(inheritanceType))
							break;

						listTypes.Add(inheritanceType);
					}
					else {
						tree.Add(baseType,
							inheritanceType == null
								? new List<TypeDefinition>()
								: new List<TypeDefinition> { inheritanceType });
					}

					inheritanceType = baseType;
					baseType = baseType.BaseType?.Resolve();
				}
			}

			return new TypeDefinitionsTree(tree);
		}

		public static IEnumerable<TypeDefinition> WhereFrom(this IEnumerable<TypeDefinition> types, ModuleDefinition module) {
			return types.Where(type => type.Module == module);
		}
	}
}