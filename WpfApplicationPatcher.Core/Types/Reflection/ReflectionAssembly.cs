using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionAssembly : ObjectBase<Assembly[]> {
		public ReflectionAssembly(Assembly[] instance) : base(instance) {
		}

		[CanBeNull]
		public ReflectionType GetReflectionTypeByName(string typeFullName) {
			return Instance.Select(assembly => assembly.GetType(typeFullName)).FirstOrDefault(type => type != null)?.ToReflectionType();
		}
	}
}