using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonProperty {
		public readonly string FullName;
		public readonly CommonAttribute[] Attributes;
		public readonly MonoCecilProperty MonoCecilProperty;
		public readonly ReflectionProperty ReflectionProperty;

		internal CommonProperty(string fullName, CommonAttribute[] attributes, MonoCecilProperty monoCecilProperty, ReflectionProperty reflectionProperty) {
			FullName = fullName;
			Attributes = attributes;
			MonoCecilProperty = monoCecilProperty;
			ReflectionProperty = reflectionProperty;
		}
	}
}