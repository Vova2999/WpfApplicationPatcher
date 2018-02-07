using System;
using System.Linq;
using System.Reflection;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class ReflectionAssembly {
		public readonly Assembly MainAssembly;
		public readonly Assembly[] ReferencedAssemblies;

		public ReflectionAssembly(Assembly mainAssembly, Assembly[] referencedAssemblies) {
			MainAssembly = mainAssembly;
			ReferencedAssemblies = referencedAssemblies;
		}

		public Type GetReflectionTypeByName(string typeFullName) {
			return MainAssembly.GetType(typeFullName) ??
				ReferencedAssemblies
					.Select(referencedAssembly => referencedAssembly.GetType(typeFullName))
					.FirstOrDefault(referencedType => referencedType != null);
		}
	}
}