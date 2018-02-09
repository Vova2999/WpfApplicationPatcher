using System.Linq;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Factories;
using WpfApplicationPatcher.Core.Helpers;

namespace WpfApplicationPatcher {
	public class WpfApplicationPatcherProcessor {
		private readonly ReflectionAssemblyFactory reflectionAssemblyFactory;
		private readonly AssemblyDefinitionFactory assemblyDefinitionFactory;
		private readonly AssemblyContainerFactory assemblyContainerFactory;
		private readonly IPatcher[] patchers;
		private readonly Log log;

		public WpfApplicationPatcherProcessor(ReflectionAssemblyFactory reflectionAssemblyFactory,
											  AssemblyDefinitionFactory assemblyDefinitionFactory,
											  AssemblyContainerFactory assemblyContainerFactory,
											  IPatcher[] patchers) {
			this.reflectionAssemblyFactory = reflectionAssemblyFactory;
			this.assemblyDefinitionFactory = assemblyDefinitionFactory;
			this.assemblyContainerFactory = assemblyContainerFactory;
			this.patchers = patchers;
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void PatchApplication(string wpfApplicationPath) {
			log.Info("Reading assembly...");
			var reflectionAssembly = reflectionAssemblyFactory.Craete(wpfApplicationPath);
			var monoCecilAssembly = assemblyDefinitionFactory.Create(wpfApplicationPath);
			log.Info("Assembly was readed");

			log.Info("Building assembly container...");
			var assemblyContainer = assemblyContainerFactory.Create(reflectionAssembly, monoCecilAssembly);
			log.Info("Assembly container was built");

			log.Debug("Types found:", assemblyContainer.CommonAssemblyTypes.Select(assemblyType => assemblyType.FullName));

			log.Info("Patching application...");
			patchers.ForEach(patcher => patcher.Patch(monoCecilAssembly, assemblyContainer));
			log.Info("Application was patched");

			log.Info("Write assembly...");
			assemblyDefinitionFactory.Write(monoCecilAssembly, wpfApplicationPath);
			log.Info("Assembly was recorded");
		}
	}
}