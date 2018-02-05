using System;
using Mono.Cecil;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class AssemblyAttributeType {
		public readonly Attribute ReflectionAttribute;
		public readonly CustomAttribute MonoCecilAttribute;

		public AssemblyAttributeType(Attribute reflectionAttribute, CustomAttribute monoCecilAttribute) {
			ReflectionAttribute = reflectionAttribute;
			MonoCecilAttribute = monoCecilAttribute;
		}
	}
}