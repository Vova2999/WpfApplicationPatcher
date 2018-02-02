using log4net;
using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;
using WpfApplicationPatcher.Patchers;

namespace WpfApplicationPatcher {
	public class WpfApplicationPatcherProcessor {
		private readonly IPatcher[] patchers;
		private readonly ILog log;

		public WpfApplicationPatcherProcessor(IPatcher[] patchers) {
			this.patchers = patchers;
			log = Log.For(this);
		}

		public void PatchApplication(string wpfApplicationPath) {
			log.Info("Reading assembly...");
			var assembly = AssemblyDefinition.ReadAssembly(wpfApplicationPath, new ReaderParameters { ReadSymbols = true });
			log.Info("Assembly was readed");

			var module = assembly.MainModule;

			log.Info("Building tree...");
			var tree = module.Types.ToTree();
			log.Info("Tree was built");

			log.Info("Patching application...");
			patchers.ForEach(patcher => patcher.Patch(module, tree));
			log.Info("Application was patched");

			log.Info("Write assembly...");
			assembly.Write(wpfApplicationPath, new WriterParameters { WriteSymbols = true });
			log.Info("Assembly was recorded");
		}
	}
}