using System;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Types.Attributes.Classes {
	[AttributeUsage(AttributeTargets.Class)]
	public class PatchingViewModelAttribute : Attribute {
		private readonly ViewModelPatchingType viewModelPatchingType;

		public PatchingViewModelAttribute(ViewModelPatchingType viewModelPatchingType = ViewModelPatchingType.All) {
			this.viewModelPatchingType = viewModelPatchingType;
		}
	}
}