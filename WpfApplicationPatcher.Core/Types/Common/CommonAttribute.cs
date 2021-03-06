using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonAttribute {
		public string Name => MonoCecilAttribute.Name;
		public string FillName => MonoCecilAttribute.FullName;
		public readonly MonoCecilAttribute MonoCecilAttribute;
		public readonly ReflectionAttribute ReflectionAttribute;

		internal CommonAttribute(MonoCecilAttribute monoCecilAttribute, ReflectionAttribute reflectionAttribute) {
			MonoCecilAttribute = monoCecilAttribute;
			ReflectionAttribute = reflectionAttribute;
		}
	}
}