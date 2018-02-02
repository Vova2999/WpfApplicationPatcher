using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace WpfApplicationPatcher.Extensions {
	public static class CustomAttributeExtensions {
		public static bool Is(this CustomAttribute attribute, string typeName) {
			return attribute.AttributeType.Resolve().FullName == typeName;
		}

		public static bool IsNot(this CustomAttribute attribute, string typeName) {
			return !attribute.Is(typeName);
		}

		public static bool Contains(this IEnumerable<CustomAttribute> attributes, string typeName) {
			return attributes.Any(attribute => attribute.Is(typeName));
		}

		public static bool NotContains(this IEnumerable<CustomAttribute> attributes, string typeName) {
			return !attributes.Contains(typeName);
		}
	}
}