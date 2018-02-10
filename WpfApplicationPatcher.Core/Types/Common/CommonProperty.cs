using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonProperty {
		public string Name => MonoCecilProperty.Name;
		public string FullName => MonoCecilProperty.FullName;
		public readonly CommonAttribute[] Attributes;
		public readonly MonoCecilProperty MonoCecilProperty;
		public readonly ReflectionProperty ReflectionProperty;

		internal CommonProperty(CommonAttribute[] attributes, MonoCecilProperty monoCecilProperty, ReflectionProperty reflectionProperty) {
			Attributes = attributes;
			MonoCecilProperty = monoCecilProperty;
			ReflectionProperty = reflectionProperty;
		}
	}
}