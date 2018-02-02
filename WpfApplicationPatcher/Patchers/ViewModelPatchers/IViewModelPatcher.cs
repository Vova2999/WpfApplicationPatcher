using Mono.Cecil;

namespace WpfApplicationPatcher.Patchers.ViewModelPatchers {
	public interface IViewModelPatcher {
		void Patch(ModuleDefinition module, TypeDefinition viewModelBaseType, TypeDefinition viewModelType);
	}
}