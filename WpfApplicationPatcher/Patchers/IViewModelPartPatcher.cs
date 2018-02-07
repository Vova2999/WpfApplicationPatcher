using WpfApplicationPatcher.Types.Common;
using WpfApplicationPatcher.Types.Enums;
using WpfApplicationPatcher.Types.MonoCecil;

namespace WpfApplicationPatcher.Patchers {
	public interface IViewModelPartPatcher {
		void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBaseAssemblyType, CommonType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType);
	}
}