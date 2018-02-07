using Mono.Cecil;

namespace WpfApplicationPatcher.Factories {
	public class AssemblyDefinitionFactory {
		public AssemblyDefinition Create(string assemblyPath) {
			return AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { ReadSymbols = true });
		}

		public void Write(AssemblyDefinition monoCecilAssembly, string assemblyPath) {
			monoCecilAssembly.Write(assemblyPath, new WriterParameters { WriteSymbols = true });
		}
	}
}