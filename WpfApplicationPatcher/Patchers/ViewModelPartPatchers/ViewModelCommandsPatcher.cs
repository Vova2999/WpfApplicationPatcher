using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Helpers;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Types.Attributes.Properties;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers.ViewModelPartPatchers {
	public class ViewModelCommandsPatcher : IViewModelPartPatcher {
		private readonly Log log;

		public ViewModelCommandsPatcher() {
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBase, CommonType viewModel, ViewModelPatchingType viewModelPatchingType) {
			log.Info($"Patching {viewModel.FullName} commands...");

			var viewModelCommands = GetViewModelCommands(viewModel, viewModelPatchingType);
		}

		[DoNotAddLogOffset]
		private CommonProperty[] GetViewModelCommands(CommonType viewModel, ViewModelPatchingType viewModelPatchingType) {
			switch (viewModelPatchingType) {
				case ViewModelPatchingType.All:
					return viewModel.Properties
						.Where(property =>
							property.Attributes.NotContains(typeof(NotPatchingPropertyAttribute)) &&
							(property.Attributes.Contains(typeof(PatchingPropertyAttribute)) || property.Is(typeof(ICommand))))
						.ToArray();
				case ViewModelPatchingType.Selectively:
					return viewModel.Properties
						.Where(property => property.Attributes.Contains(typeof(PatchingPropertyAttribute)))
						.ToArray();
				default:
					log.Error($"Not implement patching for commands with {nameof(ViewModelPatchingType)} = {viewModelPatchingType}");
					throw new ArgumentOutOfRangeException(nameof(viewModelPatchingType), viewModelPatchingType, null);
			}
		}

		private class CommandMembers {
			public PropertyInfo CommandPropertyInfo { get; set; }
			public MethodInfo ExecuteMethodInfo { get; set; }
			public MethodInfo CanExecuteMethodInfo { get; set; }
		}
	}
}