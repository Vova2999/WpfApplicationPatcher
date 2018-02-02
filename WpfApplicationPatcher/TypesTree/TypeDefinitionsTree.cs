using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mono.Cecil;

namespace WpfApplicationPatcher.TypesTree {
	public class TypeDefinitionsTree {
		private readonly Dictionary<TypeDefinition, List<TypeDefinition>> tree;

		public TypeDefinitionsTree(Dictionary<TypeDefinition, List<TypeDefinition>> tree) {
			this.tree = tree;
		}
		
		[NotNull]
		public TypeDefinition GetTypeByName(string typeFullName) {
			var types = tree.Keys.Where(type => type.FullName.Equals(typeFullName, StringComparison.InvariantCultureIgnoreCase)).ToArray();
			if (!types.Any())
				throw new ArgumentException($"Not found type '{typeFullName}'");
			if (types.Length > 1)
				throw new ArgumentException($"More than one type '{typeFullName}' was found");

			return types.Single();
		}

		[NotNull]
		public IEnumerable<TypeDefinition> GetAllInheritanceTypes(TypeDefinition type) {
			return tree.TryGetValue(type, out var listTypes)
				? listTypes.Concat(listTypes.SelectMany(GetAllInheritanceTypes))
				: throw new ArgumentException($"Not found type '{type.FullName}'");
		}

		[NotNull]
		public IEnumerable<TypeDefinition> GetDirectInheritanceTypes(TypeDefinition type) {
			return tree.TryGetValue(type, out var listTypes)
				? listTypes
				: throw new ArgumentException($"Not found type '{type.FullName}'");
		}
	}
}