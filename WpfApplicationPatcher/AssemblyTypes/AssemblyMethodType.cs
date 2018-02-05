using System.Reflection;
using Mono.Cecil;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class AssemblyMethodType {
		private readonly string fullName;
		public readonly MethodInfo ReflectionMethod;
		public readonly MethodDefinition MonoCecilMethod;
		public readonly AssemblyAttributeType[] Attributes;

		public AssemblyMethodType(string fullName, MethodInfo reflectionMethod, MethodDefinition monoCecilMethod, AssemblyAttributeType[] attributes) {
			this.fullName = fullName;
			ReflectionMethod = reflectionMethod;
			MonoCecilMethod = monoCecilMethod;
			Attributes = attributes;
		}
	}
}