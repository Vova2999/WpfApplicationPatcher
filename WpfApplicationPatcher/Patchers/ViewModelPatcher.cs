using System.Linq;
using log4net;
using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;
using WpfApplicationPatcher.Patchers.ViewModelPatchers;
using WpfApplicationPatcher.TypesTree;

namespace WpfApplicationPatcher.Patchers {
	public class ViewModelPatcher : IPatcher {
		private readonly IViewModelPatcher[] viewModelPatchers;
		private readonly ILog log;

		public ViewModelPatcher(IViewModelPatcher[] viewModelPatchers) {
			this.viewModelPatchers = viewModelPatchers;
			log = Log.For(this);
		}

		public void Patch(ModuleDefinition module, TypeDefinitionsTree tree) {
			log.Info("Patching view models...");

			var viewModelBaseType = tree.GetTypeByName(TypeNames.ViewModelBase);
			var viewModelTypes = tree.GetAllInheritanceTypes(viewModelBaseType).WhereFrom(module).ToArray();
			log.Debug($"View models found:\r\n{string.Join("\r\n", viewModelTypes.Select((viewModelType, index) => $"\t{index + 1}) {viewModelType.FullName}"))}");

			foreach (var viewModelType in viewModelTypes) {
				log.Info($"Patching {viewModelType.FullName}...");
				viewModelPatchers.ForEach(viewModelPatcher => viewModelPatcher.Patch(module, viewModelBaseType, viewModelType));
				log.Info($"{viewModelType.FullName} was patched");
			}

			log.Info("View models was patched");
		}
	}
}