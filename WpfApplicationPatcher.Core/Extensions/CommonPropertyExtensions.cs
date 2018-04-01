using System;
using System.Linq;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Extensions {
	// ReSharper disable UnusedMember.Global

	public static class CommonPropertyExtensions {
		public static bool Is(this CommonProperty commonProperty, Type type) {
			return commonProperty.ReflectionProperty.PropertyType.Instance == type;
		}

		public static bool IsNot(this CommonProperty commonProperty, Type type) {
			return commonProperty.ReflectionProperty.PropertyType.Instance == type;
		}

		public static bool IsInheritedFrom(this CommonProperty commonProperty, Type type) {
			return type.IsAssignableFrom(commonProperty.ReflectionProperty.PropertyType.Instance);
		}

		public static bool IsNotInheritedFrom(this CommonProperty commonProperty, Type type) {
			return !commonProperty.IsInheritedFrom(type);
		}

		public static TAttribute GetReflectionAttribute<TAttribute>(this CommonProperty commonProperty) where TAttribute : Attribute {
			return (TAttribute)commonProperty.ReflectionProperty.Attributes.FirstOrDefault(attribute => attribute.AttributeType.Instance == typeof(TAttribute))?.Instance;
		}
	}
}