using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.MonoCecil;

namespace WpfApplicationPatcher.Core.Factories {
	public class MonoCecilAssemblyFactory {
		public virtual MonoCecilAssembly Create(string assemblyPath) {
			return AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { ReadSymbols = true }).ToMonoCecilAssembly();
		}

		public virtual void Save(MonoCecilAssembly monoCecilAssembly, string assemblyPath) {
			monoCecilAssembly.Instance.Write(assemblyPath, new WriterParameters { WriteSymbols = true });
		}
	}
}