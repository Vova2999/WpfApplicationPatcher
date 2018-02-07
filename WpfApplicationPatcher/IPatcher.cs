using Mono.Cecil;
using WpfApplicationPatcher.Types;
using WpfApplicationPatcher.Types.MonoCecil;

namespace WpfApplicationPatcher {
	public interface IPatcher {
		void Patch(MonoCecilAssembly monoCecilAssembly, CommonAssemblyContainer assemblyContainer);
	}
}