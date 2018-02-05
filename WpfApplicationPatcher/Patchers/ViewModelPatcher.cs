using System.Linq;
using GalaSoft.MvvmLight;
using Mono.Cecil;
using WpfApplicationPatcher.AssemblyTypes;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;
using WpfApplicationPatcher.Types.Attributes.Classes;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers {
	public class ViewModelPatcher : IPatcher {
		private readonly IViewModelPartPatcher[] viewModelPartPatchers;
		private readonly Log log;

		public ViewModelPatcher(IViewModelPartPatcher[] viewModelPartPatchers) {
			this.viewModelPartPatchers = viewModelPartPatchers;
			log = Log.For(this, 1);
		}

		public void Patch(AssemblyDefinition monoCecilAssembly, AssemblyContainer assemblyContainer) {
			log.Info("Patching view models...");

			var viewModelBaseAssemblyType = assemblyContainer.GetAssemblyTypeByReflectionType(typeof(ViewModelBase)).Load();
			var viewModelAssemblyTypes = assemblyContainer.GetInheritanceAssemblyTypes(viewModelBaseAssemblyType)
				.WhereFrom(monoCecilAssembly.MainModule)
				.Select(viewModelAssemblyType => viewModelAssemblyType.Load())
				.ToArray();
			log.Debug("Types found:", viewModelAssemblyTypes.Select(viewModelType => viewModelType.FullName));

			foreach (var viewModelAssemblyType in viewModelAssemblyTypes) {
				log.Info($"Patching {viewModelAssemblyType.FullName}...");

				var patchingViewModelAttribute = viewModelAssemblyType.GetReflectionAttribute<PatchingViewModelAttribute>();
				var viewModelPatchingType = patchingViewModelAttribute?.ViewModelPatchingType ?? ViewModelPatchingType.All;
				log.Info($"View model patching type: {viewModelPatchingType}");

				viewModelPartPatchers.ForEach(viewModelPatcher => viewModelPatcher.Patch(monoCecilAssembly, viewModelBaseAssemblyType, viewModelAssemblyType, viewModelPatchingType));
				log.Info($"{viewModelAssemblyType.FullName} was patched");
			}

			log.Info("View models was patched");
		}
	}
}