using System.Collections.Generic;
using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionField : FieldBase<FieldInfo, ReflectionAttribute> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override string FullName => GetOrCreate(() => Instance.DeclaringType == null ? Name : $"{Instance.DeclaringType?.FullName}.{Name}");
		public override IEnumerable<ReflectionAttribute> Attributes => GetOrCreate(() => Instance.GetCustomAttributes().ToReflectionAttributes());

		internal ReflectionField(FieldInfo instance) : base(instance) {
		}
	}
}