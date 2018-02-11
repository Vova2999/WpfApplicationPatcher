using System;
using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;

namespace WpfApplicationPatcher.Core.Extensions {
	// ReSharper disable UnusedMember.Global

	public static class CommonTypeExtensions {
		public static bool Is(this CommonType commonType, Type type) {
			return type.IsAssignableFrom(commonType.ReflectionType.Instance);
		}

		public static bool IsNot(this CommonType commonType, Type type) {
			return !commonType.Is(type);
		}

		public static TAttribute GetReflectionAttribute<TAttribute>(this CommonType commonType) where TAttribute : Attribute {
			return (TAttribute)commonType.ReflectionType.Attributes.FirstOrDefault(attribute => attribute.AttributeType.Instance == typeof(TAttribute))?.Instance;
		}

		public static IEnumerable<CommonType> WhereFrom(this IEnumerable<CommonType> commonTypes, MonoCecilModule monoCecilModule) {
			return commonTypes.Where(commonType => commonType.MonoCecilType.Module == monoCecilModule);
		}
	}
}