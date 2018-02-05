using System.Reflection;
using Mono.Cecil;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class AssemblyPropertyType {
		public readonly string FullName;
		public readonly PropertyInfo ReflectionProperty;
		public readonly PropertyDefinition MonoCecilProperty;
		public readonly AssemblyAttributeType[] Attributes;

		public AssemblyPropertyType(string fullName, PropertyInfo reflectionProperty, PropertyDefinition monoCecilProperty, AssemblyAttributeType[] attributes) {
			FullName = fullName;
			ReflectionProperty = reflectionProperty;
			MonoCecilProperty = monoCecilProperty;
			Attributes = attributes;
		}
	}
}