using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.Reflection {
	public class ReflectionAssembly : ObjectBase<Assembly[]> {
		public ReflectionAssembly(Assembly[] instance) : base(instance) {
		}

		[CanBeNull]
		public ReflectionType GetReflectionTypeByName(string typeFullName) {
			return Instance.Select(assembly => assembly.GetType(typeFullName)).FirstOrDefault(type => type != null)?.ToReflectionType();
		}
	}
}