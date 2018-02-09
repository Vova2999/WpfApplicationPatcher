using System.Collections.Generic;
using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionProperty : PropertyBase<PropertyInfo, ReflectionMethod, ReflectionType, ReflectionAttribute> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override string FullName => GetOrCreate(() => Instance.DeclaringType == null ? Name : $"{Instance.DeclaringType?.FullName}.{Name}");
		public override ReflectionMethod GetMethod => GetOrCreate(() => Instance.GetMethod.ToReflectionMethod());
		public override ReflectionMethod SetMethod => GetOrCreate(() => Instance.SetMethod.ToReflectionMethod());
		public override ReflectionType PropertyType => GetOrCreate(() => Instance.PropertyType.ToReflectionType());
		public override IEnumerable<ReflectionAttribute> Attributes => GetOrCreate(() => Instance.GetCustomAttributes().ToReflectionAttributes());

		internal ReflectionProperty(PropertyInfo instance) : base(instance) {
		}
	}
}