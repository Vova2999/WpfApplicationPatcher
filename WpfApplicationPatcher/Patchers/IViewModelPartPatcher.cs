using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers {
	public interface IViewModelPartPatcher {
		void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBaseAssemblyType, CommonType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType);
	}
}