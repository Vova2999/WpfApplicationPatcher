using System.Reflection;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.Reflection {
	public class ReflectionParameter : ObjectBase<ParameterInfo> {
		public ReflectionType ParameterType => Instance.ParameterType.ToReflectionType();

		public ReflectionParameter(ParameterInfo instance) : base(instance) {
		}
	}
}