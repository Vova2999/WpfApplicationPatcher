using WpfApplicationPatcher.Types.MonoCecil;
using WpfApplicationPatcher.Types.Reflection;

namespace WpfApplicationPatcher.Types.Common {
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