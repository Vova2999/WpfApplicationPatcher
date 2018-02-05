using Mono.Cecil;
using WpfApplicationPatcher.AssemblyTypes;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers {
	public interface IViewModelPartPatcher {
		void Patch(AssemblyDefinition monoCecilAssembly, AssemblyType viewModelBaseAssemblyType, AssemblyType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType);
	}
}