using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionParameter : ParameterBase<ParameterInfo, ReflectionType> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override ReflectionType ParameterType => GetOrCreate(() => Instance.ParameterType.ToReflectionType());

		internal ReflectionParameter(ParameterInfo instance) : base(instance) {
		}
	}
}