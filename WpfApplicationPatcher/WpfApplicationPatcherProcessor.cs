using System.Linq;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Factories;
using WpfApplicationPatcher.Core.Helpers;

namespace WpfApplicationPatcher {
	public class WpfApplicationPatcherProcessor {
		private readonly ReflectionAssemblyFactory reflectionAssemblyFactory;
		private readonly MonoCecilAssemblyFactory monoCecilAssemblyFactory;
		private readonly CommonAssemblyContainerFactory commonAssemblyContainerFactory;
		private readonly IPatcher[] patchers;
		private readonly Log log;

		public WpfApplicationPatcherProcessor(ReflectionAssemblyFactory reflectionAssemblyFactory,
											  MonoCecilAssemblyFactory monoCecilAssemblyFactory,
											  CommonAssemblyContainerFactory commonAssemblyContainerFactory,
											  IPatcher[] patchers) {
			this.reflectionAssemblyFactory = reflectionAssemblyFactory;
			this.monoCecilAssemblyFactory = monoCecilAssemblyFactory;
			this.commonAssemblyContainerFactory = commonAssemblyContainerFactory;
			this.patchers = patchers;
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void PatchApplication(string wpfApplicationPath) {
			log.Info("Reading assembly...");
			var reflectionAssembly = reflectionAssemblyFactory.Create(wpfApplicationPath);
			var monoCecilAssembly = monoCecilAssemblyFactory.Create(wpfApplicationPath);
			log.Info("Assembly was readed");

			log.Info("Building assembly container...");
			var assemblyContainer = commonAssemblyContainerFactory.Create(reflectionAssembly, monoCecilAssembly);
			log.Info("Assembly container was built");

			var assemblyContainerFullNames = assemblyContainer.FullNames.ToArray();
			if (!assemblyContainerFullNames.Any()) {
				log.Debug("Types not found");
				return;
			}

			log.Debug("Types found:", assemblyContainerFullNames);

			log.Info("Patching application...");
			patchers.ForEach(patcher => patcher.Patch(monoCecilAssembly, assemblyContainer));
			log.Info("Application was patched");

			log.Info("Save assembly...");
			monoCecilAssemblyFactory.Save(monoCecilAssembly, wpfApplicationPath);
			log.Info("Assembly was saved");
		}
	}
}