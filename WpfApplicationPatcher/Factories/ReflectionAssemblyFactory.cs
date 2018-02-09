using System;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Reflection;

namespace WpfApplicationPatcher.Factories {
	public class ReflectionAssemblyFactory {
		public virtual ReflectionAssembly Craete([NotNull] string assemblyPath) {
			var symbolStorePath = Path.ChangeExtension(assemblyPath, "pdb");

			var rawAssembly = File.ReadAllBytes(assemblyPath);
			var rawSymbolStore = File.ReadAllBytes(symbolStorePath);
			File.Delete(symbolStorePath);

			var mainAssembly = Assembly.Load(rawAssembly, rawSymbolStore);
			File.WriteAllBytes(symbolStorePath, rawSymbolStore);

			var foundedAssemblyFiles = Directory.GetFiles(Directory.GetCurrentDirectory())
				.GroupBy(Path.GetFileNameWithoutExtension)
				.ToDictionary(group => group.Key, group => group.SingleOrDefault(path => Path.GetExtension(path) == ".exe" || Path.GetExtension(path) == ".dll"));

			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
				foundedAssemblyFiles.TryGetValue(new AssemblyName(args.Name).Name, out var assemblyFile) ? Assembly.Load(File.ReadAllBytes(assemblyFile)) : null;

			return new[] { mainAssembly }.Concat(mainAssembly.GetReferencedAssemblies().Select(Assembly.Load)).ToReflectionAssembly();
		}
	}
}