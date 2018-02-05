using Mono.Cecil;
using WpfApplicationPatcher.AssemblyTypes;

namespace WpfApplicationPatcher {
	public interface IPatcher {
		void Patch(AssemblyDefinition monoCecilAssembly, AssemblyContainer assemblyContainer);
	}
}