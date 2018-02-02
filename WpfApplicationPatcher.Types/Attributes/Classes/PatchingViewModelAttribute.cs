﻿using System;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Types.Attributes.Classes {
	[AttributeUsage(AttributeTargets.Class)]
	public class PatchingViewModelAttribute : Attribute {
		public readonly ViewModelPatchingType ViewModelPatchingType;

		public PatchingViewModelAttribute(ViewModelPatchingType viewModelPatchingType = ViewModelPatchingType.All) {
			ViewModelPatchingType = viewModelPatchingType;
		}
	}
}