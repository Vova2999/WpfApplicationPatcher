using System.Reflection;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionField : FieldBase<FieldInfo> {
		public override string Name => GetOrCreate(() => Instance.Name);

		internal ReflectionField(FieldInfo instance) : base(instance) {
		}
	}
}