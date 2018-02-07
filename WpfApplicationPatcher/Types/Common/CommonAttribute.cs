using WpfApplicationPatcher.Types.MonoCecil;
using WpfApplicationPatcher.Types.Reflection;

namespace WpfApplicationPatcher.Types.Common {
	public class CommonAttribute {
		public readonly MonoCecilAttribute MonoCecilAttribute;
		public readonly ReflectionAttribute ReflectionAttribute;

		public CommonAttribute(MonoCecilAttribute monoCecilAttribute, ReflectionAttribute reflectionAttribute) {
			MonoCecilAttribute = monoCecilAttribute;
			ReflectionAttribute = reflectionAttribute;
		}
	}
}