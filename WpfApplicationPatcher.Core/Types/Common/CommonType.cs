using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonType {
		public readonly string FullName;
		public readonly MonoCecilType MonoCecilType;
		public readonly ReflectionType ReflectionType;

		public bool IsLoaded { get; private set; }
		public CommonMethod[] Methods { get; private set; }
		public CommonProperty[] Properties { get; private set; }
		public CommonAttribute[] Attributes { get; private set; }

		public CommonType(string fullName, MonoCecilType monoCecilType, ReflectionType reflectionType) {
			FullName = fullName;
			MonoCecilType = monoCecilType;
			ReflectionType = reflectionType;
		}

		public CommonType Load() {
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
			var reflectionMethods = ReflectionType.GetMethods(bindingFlags).ToArray();
			Methods = MonoCecilType.Methods
				.Where(monoCecilMethod => monoCecilMethod.Name != ".ctor" && !monoCecilMethod.Name.StartsWith("get_") && !monoCecilMethod.Name.StartsWith("set_"))
				.Select(monoCecilMethod => CreateAssemblyMethodType(monoCecilMethod, reflectionMethods))
				.Where(assemblyMethodType => assemblyMethodType != null)
				.ToArray();
		}

		private static CommonMethod CreateAssemblyMethodType(MonoCecilMethod monoCecilMethod, IEnumerable<ReflectionMethod> reflectionMethods) {
			var reflectionMethod = reflectionMethods.FirstOrDefault(methodInfo =>
				methodInfo.Name == monoCecilMethod.Name &&
				methodInfo.GetParameters()
					.Select(reflectionParameter => reflectionParameter.ParameterType.FullName)
					.SequenceEqual(monoCecilMethod.Parameters.Select(parameter => parameter.ParameterType.FullName)));

			if (reflectionMethod == null)
				return null;

			var reflectionAttributes = reflectionMethod.GetCustomAttributes();
			var assemblyAttributeTypes = monoCecilMethod.CustomAttributes
				.Select(attribute => new CommonAttribute(
					attribute,
					reflectionAttributes.FirstOrDefault(reflectionAttribute => reflectionAttribute.FullName == attribute.AttributeType.FullName)))
				.Where(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute != null)
				.ToArray();

			return new CommonMethod(monoCecilMethod.FullName, assemblyAttributeTypes, monoCecilMethod, reflectionMethod);
		}

		private void LoadProperties() {
			const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var reflectionProperties = ReflectionType.GetProperties(bindingFlags);
			Properties = MonoCecilType.Properties
				.Select(property => CreateAssemblyPropertyType(property, reflectionProperties))
				.Where(assemblyPropertyType => assemblyPropertyType != null)
				.ToArray();
		}

		private static CommonProperty CreateAssemblyPropertyType(MonoCecilProperty monoCecilProperty, IEnumerable<ReflectionProperty> reflectionProperties) {
			var reflectionProperty = reflectionProperties.FirstOrDefault(propertyInfo => propertyInfo.Name == monoCecilProperty.Name);

			if (reflectionProperty == null)
				return null;

			var reflectionAttributes = reflectionProperty.GetCustomAttributes();
			var assemblyAttributeTypes = monoCecilProperty.CustomAttributes
				.Select(attribute => new CommonAttribute(
					attribute,
					reflectionAttributes.FirstOrDefault(reflectionAttribute => reflectionAttribute.FullName == attribute.AttributeType.FullName)))
				.Where(assemblyAttributeType => assemblyAttributeType.ReflectionAttribute != null)
				.ToArray();

			return new CommonProperty(monoCecilProperty.FullName, assemblyAttributeTypes, monoCecilProperty, reflectionProperty);
		}

		private void LoadAttributes() {
			var reflectionAttributes = ReflectionType.GetCustomAttributes();
			Attributes = MonoCecilType.CustomAttributes
				.Select(attribute => new CommonAttribute(
					attribute,
					reflectionAttributes.FirstOrDefault(reflectionAttribute => reflectionAttribute.FullName == attribute.AttributeType.FullName)))
				.Where(assemblyAttributeTypes => assemblyAttributeTypes.ReflectionAttribute != null)
				.ToArray();
		}
	}
}