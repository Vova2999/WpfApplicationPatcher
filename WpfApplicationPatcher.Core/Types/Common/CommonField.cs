using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonField {
		public string Name => MonoCecilField.Name;
		public readonly CommonAttribute[] Attributes;
		public readonly MonoCecilField MonoCecilField;
		public readonly ReflectionField ReflectionField;

		internal CommonField(CommonAttribute[] attributes, MonoCecilField monoCecilField, ReflectionField reflectionField) {
			Attributes = attributes;
			MonoCecilField = monoCecilField;
			ReflectionField = reflectionField;
		}
	}
}