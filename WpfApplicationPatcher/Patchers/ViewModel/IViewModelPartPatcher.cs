﻿using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers.ViewModel {
	public interface IViewModelPartPatcher {
		void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBase, CommonType viewModel, ViewModelPatchingType viewModelPatchingType);
	}
}