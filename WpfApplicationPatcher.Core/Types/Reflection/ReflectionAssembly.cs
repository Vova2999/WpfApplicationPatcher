using System.Linq;
using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionAssembly : AssemblyBase<Assembly[], ReflectionType> {
		internal ReflectionAssembly(Assembly[] instance) : base(instance) {
		}

		public override ReflectionType GetTypeByName(string typeFullName) {
			return Instance.Select(assembly => assembly.GetType(typeFullName)).FirstOrDefault(type => type != null)?.ToReflectionType();
		}
	}
}