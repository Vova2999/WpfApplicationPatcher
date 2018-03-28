using System.Linq;
using GalaSoft.MvvmLight;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Helpers;
using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Types.Attributes.ViewModels;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers.ViewModel {
	public class ViewModelPatcher : IPatcher {
		private readonly IViewModelPartPatcher[] viewModelPartPatchers;
		private readonly Log log;

		public ViewModelPatcher(IViewModelPartPatcher[] viewModelPartPatchers) {
			this.viewModelPartPatchers = viewModelPartPatchers;
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(MonoCecilAssembly monoCecilAssembly, CommonTypeContainer commonTypeContainer) {
			log.Info("Patching view models...");

			var viewModels = commonTypeContainer.GetInheritanceCommonTypes(typeof(ViewModelBase)).WhereFrom(monoCecilAssembly.MainModule).ToArray();
			if (!viewModels.Any()) {
				log.Info("Not found view models");
				return;
			}

			var viewModelBase = commonTypeContainer.GetCommonType(typeof(ViewModelBase)).Load();
			log.Debug("View models found:", viewModels.Select(viewModel => viewModel.FullName));

			foreach (var viewModel in viewModels) {
				log.Info($"Patching {viewModel.FullName}...");
				viewModel.Load();

				var patchingViewModelAttribute = viewModel.GetReflectionAttribute<PatchingViewModelAttribute>();
				var viewModelPatchingType = patchingViewModelAttribute?.ViewModelPatchingType ?? ViewModelPatchingType.All;
				log.Info($"View model patching type: {viewModelPatchingType}");

				viewModelPartPatchers.ForEach(viewModelPartPatcher => viewModelPartPatcher.Patch(monoCecilAssembly, viewModelBase, viewModel, viewModelPatchingType));
				log.Info($"{viewModel.FullName} was patched");
			}

			log.Info("View models was patched");
		}
	}
}