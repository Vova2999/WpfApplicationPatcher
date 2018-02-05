using System;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class ReflectionAssembly {
		public readonly Assembly MainAssembly;
		public readonly Assembly[] ReferencedAssemblies;

		public ReflectionAssembly(Assembly mainAssembly, Assembly[] referencedAssemblies) {
			MainAssembly = mainAssembly;
			ReferencedAssemblies = referencedAssemblies;
		}

		public static ReflectionAssembly Load([NotNull] string assemblyPath) {
			var symbolStorePath = Path.ChangeExtension(assemblyPath, "pdb");

			var rawAssembly = File.ReadAllBytes(assemblyPath);
			var rawSymbolStore = File.ReadAllBytes(symbolStorePath);
			File.Delete(symbolStorePath);

			var mainAssembly = Assembly.Load(rawAssembly, rawSymbolStore);
			File.WriteAllBytes(symbolStorePath, rawSymbolStore);

			return new ReflectionAssembly(mainAssembly, mainAssembly.GetReferencedAssemblies().Select(name => Assembly.Load(name)).ToArray());
		}

		public Type GetReflectionTypeByName(string typeFullName) {
			return MainAssembly.GetType(typeFullName) ??
				ReferencedAssemblies
					.Select(referencedAssembly => referencedAssembly.GetType(typeFullName))
					.FirstOrDefault(referencedType => referencedType != null);
		}
	}
}