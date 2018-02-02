using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace WpfApplicationPatcher.Extensions {
	public static class TypeReferenceExtensions {
		public static bool Is(this TypeReference type, string typeName) {
			return type.FullName == typeName;
		}

		public static bool IsNot(this TypeReference type, string typeName) {
			return !type.Is(typeName);
		}

		public static bool Contains(this IEnumerable<TypeReference> types, string typeName) {
			return types.Any(type => type.Is(typeName));
		}

		public static bool NotContains(this IEnumerable<TypeReference> types, string typeName) {
			return !types.Contains(typeName);
		}
	}
}