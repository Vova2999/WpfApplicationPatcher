using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.MonoCecil;

namespace WpfApplicationPatcher.Factories {
	public class AssemblyDefinitionFactory {
		public virtual MonoCecilAssembly Create(string assemblyPath) {
			return AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { ReadSymbols = true }).ToMonoCecilAssembly();
		}

		public virtual void Write(MonoCecilAssembly monoCecilAssembly, string assemblyPath) {
			monoCecilAssembly.Instance.Write(assemblyPath, new WriterParameters { WriteSymbols = true });
		}
	}
}