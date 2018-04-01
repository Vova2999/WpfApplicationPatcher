using System;
using System.Linq;
using WpfApplicationPatcher.Core.Types.Common;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class CommonMethodExtensions {
		public static TAttribute GetReflectionAttribute<TAttribute>(this CommonMethod commonMethod) where TAttribute : Attribute {
			return (TAttribute)commonMethod.ReflectionMethod.Attributes.FirstOrDefault(attribute => attribute.AttributeType.Instance == typeof(TAttribute))?.Instance;
		}
	}
}