using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.MonoCecil;

namespace WpfApplicationPatcher {
	public interface IPatcher {
		void Patch(MonoCecilAssembly monoCecilAssembly, CommonAssemblyContainer commonAssemblyContainer);
	}
}