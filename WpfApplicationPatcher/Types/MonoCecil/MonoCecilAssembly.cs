using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilAssembly : ObjectBase<AssemblyDefinition> {
		public MonoCecilAssembly(AssemblyDefinition instance) : base(instance) {
		}
		public MonoCecilModule MainModule => Instance.MainModule.ToMonoCecilModule();
	}
}