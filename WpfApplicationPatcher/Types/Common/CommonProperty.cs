using WpfApplicationPatcher.Types.MonoCecil;
using WpfApplicationPatcher.Types.Reflection;

namespace WpfApplicationPatcher.Types.Common {
	public class CommonProperty {
		public readonly string FullName;
		public readonly CommonAttribute[] Attributes;
		public readonly MonoCecilProperty MonoCecilProperty;
		public readonly ReflectionProperty ReflectionProperty;

		public CommonProperty(string fullName, CommonAttribute[] attributes, MonoCecilProperty monoCecilProperty, ReflectionProperty reflectionProperty) {
			FullName = fullName;
			Attributes = attributes;
			MonoCecilProperty = monoCecilProperty;
			ReflectionProperty = reflectionProperty;
		}
	}
}