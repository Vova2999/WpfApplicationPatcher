using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilAssembly : ObjectBase<AssemblyDefinition> {
		public MonoCecilAssembly(AssemblyDefinition instance) : base(instance) {
		}
		public MonoCecilModule MainModule => Instance.MainModule.ToMonoCecilModule();
	}
}