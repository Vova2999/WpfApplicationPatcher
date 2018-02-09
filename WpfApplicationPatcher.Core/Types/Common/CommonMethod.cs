using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonMethod {
		public readonly string FullName;
		public readonly CommonAttribute[] Attributes;
		public readonly MonoCecilMethod MonoCecilMethod;
		public readonly ReflectionMethod ReflectionMethod;

		public CommonMethod(string fullName, CommonAttribute[] attributes, MonoCecilMethod monoCecilMethod, ReflectionMethod reflectionMethod) {
			FullName = fullName;
			Attributes = attributes;
			MonoCecilMethod = monoCecilMethod;
			ReflectionMethod = reflectionMethod;
		}
	}
}