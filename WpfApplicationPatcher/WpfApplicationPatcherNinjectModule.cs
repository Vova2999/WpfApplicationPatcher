using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace WpfApplicationPatcher {
	public class WpfApplicationPatcherNinjectModule : NinjectModule {
		public override void Load() {
			Kernel.Bind(c => c.FromThisAssembly().SelectAllClasses().BindAllInterfaces().Configure(y => y.InSingletonScope()));
			Kernel.Bind(c => c.FromThisAssembly().SelectAllClasses().BindAllBaseClasses().Configure(y => y.InSingletonScope()));
		}
	}
}