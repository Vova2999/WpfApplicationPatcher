using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionParameter : ObjectBase<ParameterInfo> {
		public ReflectionType ParameterType => Instance.ParameterType.ToReflectionType();

		public ReflectionParameter(ParameterInfo instance) : base(instance) {
		}
	}
}