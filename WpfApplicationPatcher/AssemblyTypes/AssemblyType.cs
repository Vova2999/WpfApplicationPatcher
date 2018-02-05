using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace WpfApplicationPatcher.AssemblyTypes {
	public class AssemblyType {
		public readonly string FullName;
		public readonly Type ReflectionType;
		public readonly TypeDefinition MonoCecilType;

		public bool IsLoaded { get; private set; }
		public AssemblyMethodType[] Methods { get; private set; }
		public AssemblyPropertyType[] Properties { get; private set; }
		public AssemblyAttributeType[] Attributes { get; private set; }

		public AssemblyType(string fullName, Type reflectionType, TypeDefinition monoCecilType) {
			FullName = fullName;
			ReflectionType = reflectionType;
			MonoCecilType = monoCecilType;
		}

		public AssemblyType Load() {
			if (IsLoaded)
				return this;

			LoadMethods();
			LoadProperties();
			LoadAttributes();

			IsLoaded = true;
			return this;
		}

		private void LoadMethods() {
			const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var reflectionMethods = ReflectionType.GetMethods(bindingFlags);
			Methods = MonoCecilType.Methods
				.Where(method => method.Name != ".ctor" && !method.Name.StartsWith("get_") && !method.Name.StartsWith("set_"))
				.Select(monoCecilMethod => CreateAssemblyMethodType(monoCecilMethod, reflectionMethods))
				.Where(assemblyMethodType => assemblyMethodType != null)
				.ToArray();
		}

		private static AssemblyMethodType CreateAssemblyMethodType(MethodDefinition monoCecilMethod, IEnumerable<MethodInfo> reflectionMethods) {
			var reflectionMethod = reflectionMethods.FirstOrDefault(methodInfo =>
				methodInfo.Name == monoCecilMethod.Name &&
				methodInfo.GetParameters()
					.Select(parameter => parameter.ParameterType.FullName)
					.SequenceEqual(monoCecilMethod.Parameters.Select(parameter => parameter.ParameterType.FullName)));

			if (reflectionMethod == null)
				return null;

			var reflectionAttributes = reflectionMethod.GetCustomAttributes();
			var assemblyAttributeTypes = monoCecilMethod.CustomAttributes
				.Select(attribute => new AssemblyAttributeType(
					reflectionAttributes.FirstOrDefault(reflectionAttribute => reflectionAttribute.GetType().FullName == attribute.AttributeType.FullName),
					attribute))
				.Where(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute != null)
				.ToArray();

			return new AssemblyMethodType(monoCecilMethod.FullName, reflectionMethod, monoCecilMethod, assemblyAttributeTypes);
		}

		private void LoadProperties() {
			const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var reflectionProperties = ReflectionType.GetProperties(bindingFlags);
			Properties = MonoCecilType.Properties
				.Select(property => CreateAssemblyPropertyType(property, reflectionProperties))
				.Where(assemblyPropertyType => assemblyPropertyType != null)
				.ToArray();
		}

		private static AssemblyPropertyType CreateAssemblyPropertyType(PropertyDefinition monoCecilProperty, IEnumerable<PropertyInfo> reflectionProperties) {
			var reflectionProperty = reflectionProperties.FirstOrDefault(propertyInfo => propertyInfo.Name == monoCecilProperty.Name);

			if (reflectionProperty == null)
				return null;

			var reflectionAttributes = reflectionProperty.GetCustomAttributes();
			var assemblyAttributeTypes = monoCecilProperty.CustomAttributes
				.Select(attribute => new AssemblyAttributeType(
					reflectionAttributes.FirstOrDefault(reflectionAttribute => reflectionAttribute.GetType().FullName == attribute.AttributeType.FullName),
					attribute))
				.Where(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute != null)
				.ToArray();

			return new AssemblyPropertyType(monoCecilProperty.FullName, reflectionProperty, monoCecilProperty, assemblyAttributeTypes);
		}

		private void LoadAttributes() {
			var reflectionAttributes = ReflectionType.GetCustomAttributes();
			Attributes = MonoCecilType.CustomAttributes
				.Select(attribute => new AssemblyAttributeType(
					reflectionAttributes.FirstOrDefault(reflectionAttribute => reflectionAttribute.GetType().FullName == attribute.AttributeType.FullName),
					attribute))
				.Where(assemblyAttributeTypes => assemblyAttributeTypes.ReflectionAttribute != null)
				.ToArray();
		}
	}
}