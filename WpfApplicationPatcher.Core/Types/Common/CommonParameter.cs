using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonParameter {
		public string Name => MonoCecilParameter.Name;
		public readonly MonoCecilParameter MonoCecilParameter;
		public readonly ReflectionParameter ReflectionParameter;

		public CommonParameter(MonoCecilParameter monoCecilParameter, ReflectionParameter reflectionParameter) {
			MonoCecilParameter = monoCecilParameter;
			ReflectionParameter = reflectionParameter;
		}
	}
}