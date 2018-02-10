using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonMethod {
		public string Name => MonoCecilMethod.Name;
		public string FullName => MonoCecilMethod.FullName;
		public readonly CommonAttribute[] Attributes;
		public readonly MonoCecilMethod MonoCecilMethod;
		public readonly ReflectionMethod ReflectionMethod;

		internal CommonMethod(CommonAttribute[] attributes, MonoCecilMethod monoCecilMethod, ReflectionMethod reflectionMethod) {
			Attributes = attributes;
			MonoCecilMethod = monoCecilMethod;
			ReflectionMethod = reflectionMethod;
		}
	}
}