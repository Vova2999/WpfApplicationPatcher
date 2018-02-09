using System.Linq;
using GalaSoft.MvvmLight;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;
using WpfApplicationPatcher.Types;
using WpfApplicationPatcher.Types.Attributes.ViewModels;
using WpfApplicationPatcher.Types.Enums;
using WpfApplicationPatcher.Types.MonoCecil;

namespace WpfApplicationPatcher.Patchers {
	public class ViewModelPatcher : IPatcher {
		private readonly IViewModelPartPatcher[] viewModelPartPatchers;
		private readonly Log log;

		public ViewModelPatcher(IViewModelPartPatcher[] viewModelPartPatchers) {
			this.viewModelPartPatchers = viewModelPartPatchers;
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(MonoCecilAssembly monoCecilAssembly, CommonAssemblyContainer assemblyContainer) {
			log.Info("Patching view models...");

			var viewModelAssemblyTypes = assemblyContainer.GetInheritanceAssemblyTypes(typeof(ViewModelBase)).WhereFrom(monoCecilAssembly.MainModule).ToArray();
			if (!viewModelAssemblyTypes.Any()) {
				log.Info("Not found view models");
				return;
			}

			var viewModelBaseAssemblyType = assemblyContainer.GetAssemblyTypeByReflectionType(typeof(ViewModelBase)).Load();
			log.Debug("View models found:", viewModelAssemblyTypes.Select(viewModelType => viewModelType.FullName));

			foreach (var viewModelAssemblyType in viewModelAssemblyTypes) {
				log.Info($"Patching {viewModelAssemblyType.FullName}...");
				viewModelAssemblyType.Load();

				var patchingViewModelAttribute = viewModelAssemblyType.ReflectionType.GetReflectionAttribute<PatchingViewModelAttribute>();
				var viewModelPatchingType = patchingViewModelAttribute?.ViewModelPatchingType ?? ViewModelPatchingType.All;
				log.Info($"View model patching type: {viewModelPatchingType}");

				viewModelPartPatchers.ForEach(viewModelPatcher => viewModelPatcher.Patch(monoCecilAssembly, viewModelBaseAssemblyType, viewModelAssemblyType, viewModelPatchingType));
				log.Info($"{viewModelAssemblyType.FullName} was patched");
			}

			log.Info("View models was patched");
		}
	}
}