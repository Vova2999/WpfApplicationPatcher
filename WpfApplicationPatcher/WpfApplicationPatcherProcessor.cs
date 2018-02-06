using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.AssemblyTypes;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;

namespace WpfApplicationPatcher {
	public class WpfApplicationPatcherProcessor {
		private readonly IPatcher[] patchers;
		private readonly Log log;

		public WpfApplicationPatcherProcessor(IPatcher[] patchers) {
			this.patchers = patchers;
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void PatchApplication(string wpfApplicationPath) {
			log.Info("Reading assembly...");
			var reflectionAssembly = ReflectionAssembly.Load(wpfApplicationPath);
			var monoCecilAssembly = AssemblyDefinition.ReadAssembly(wpfApplicationPath, new ReaderParameters { ReadSymbols = true });
			log.Info("Assembly was readed");

			log.Info("Building assembly container...");
			var assemblyContainer = AssemblyContainerBuilder.Build(reflectionAssembly, monoCecilAssembly);
			log.Info("Assembly container was built");

			log.Debug("Types found:", assemblyContainer.AssemblyTypes.Select(assemblyType => assemblyType.FullName));

			log.Info("Patching application...");
			patchers.ForEach(patcher => patcher.Patch(monoCecilAssembly, assemblyContainer));
			log.Info("Application was patched");

			log.Info("Write assembly...");
			monoCecilAssembly.Write(wpfApplicationPath, new WriterParameters { WriteSymbols = true });
			log.Info("Assembly was recorded");
		}
	}
}