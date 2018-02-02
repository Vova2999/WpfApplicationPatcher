using Mono.Cecil;
using WpfApplicationPatcher.TypesTree;

namespace WpfApplicationPatcher.Patchers {
	public interface IPatcher {
		void Patch(ModuleDefinition module, TypeDefinitionsTree tree);
	}
}