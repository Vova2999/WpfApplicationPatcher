using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.MonoCecil;

namespace WpfApplicationPatcher.Patchers {
	public interface IPatcher {
		void Patch(MonoCecilAssembly monoCecilAssembly, CommonTypeContainer commonTypeContainer);
	}
}