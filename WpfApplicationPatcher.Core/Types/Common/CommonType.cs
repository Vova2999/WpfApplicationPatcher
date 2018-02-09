using System.Collections.Generic;
using System.Linq;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Types.Common {
	public class CommonType {
		public readonly string FullName;
		public readonly MonoCecilType MonoCecilType;
		public readonly ReflectionType ReflectionType;

		private bool isLoaded;
		public CommonMethod[] Methods { get; private set; }
		public CommonProperty[] Properties { get; private set; }
		public CommonAttribute[] Attributes { get; private set; }

		internal CommonType(string fullName, MonoCecilType monoCecilType, ReflectionType reflectionType) {
			FullName = fullName;
			MonoCecilType = monoCecilType;
			ReflectionType = reflectionType;
		}

		public CommonType Load() {
			if (isLoaded)
				return this;

			LoadMethods();
			LoadProperties();
			LoadAttributes();

			isLoaded = true;
			return this;
		}

		private void LoadMethods() {
			Methods = MonoCecilType.Methods
				.Where(monoCecilMethod => monoCecilMethod.Name != ".ctor" && !monoCecilMethod.Name.StartsWith("get_") && !monoCecilMethod.Name.StartsWith("set_"))
				.Select(monoCecilMethod => CreateCommonMethod(monoCecilMethod, ReflectionType.Methods))
				.Where(commonMethod => commonMethod != null)
				.ToArray();
		}

		private static CommonMethod CreateCommonMethod(MonoCecilMethod monoCecilMethod, IEnumerable<ReflectionMethod> reflectionMethods) {
			var matchedReflectionMethod = GetMatchedReflectionMethod(monoCecilMethod, reflectionMethods);
			if (matchedReflectionMethod == null)
				return null;

			var methodAttributes = JoinAttributes(matchedReflectionMethod.Attributes, monoCecilMethod.Attributes);
			return new CommonMethod(monoCecilMethod.FullName, methodAttributes, monoCecilMethod, matchedReflectionMethod);
		}

		private static ReflectionMethod GetMatchedReflectionMethod(MonoCecilMethod monoCecilMethod, IEnumerable<ReflectionMethod> reflectionMethods) {
			return reflectionMethods.FirstOrDefault(reflectionMethod =>
				reflectionMethod.Name == monoCecilMethod.Name &&
				reflectionMethod.Parameters
					.Select(reflectionParameter => reflectionParameter.ParameterType.FullName)
					.SequenceEqual(monoCecilMethod.Parameters.Select(parameter => parameter.ParameterType.FullName)));
		}

		private void LoadProperties() {
			Properties = MonoCecilType.Properties
				.Select(property => CreateCommonProperty(property, ReflectionType.Properties))
				.Where(commonProperty => commonProperty != null)
				.ToArray();
		}

		private static CommonProperty CreateCommonProperty(MonoCecilProperty monoCecilProperty, IEnumerable<ReflectionProperty> reflectionProperties) {
			var reflectionProperty = reflectionProperties.FirstOrDefault(propertyInfo => propertyInfo.Name == monoCecilProperty.Name);
			if (reflectionProperty == null)
				return null;

			var propertyAttributes = JoinAttributes(reflectionProperty.Attributes, monoCecilProperty.Attributes);
			return new CommonProperty(monoCecilProperty.FullName, propertyAttributes, monoCecilProperty, reflectionProperty);
		}

		private void LoadAttributes() {
			Attributes = JoinAttributes(ReflectionType.Attributes, MonoCecilType.Attributes);
		}

		private static CommonAttribute[] JoinAttributes(IEnumerable<ReflectionAttribute> reflectionAttributes, IEnumerable<MonoCecilAttribute> monoCecilAttributes) {
			return monoCecilAttributes.Select(attribute => new CommonAttribute(
					attribute,
					reflectionAttributes.FirstOrDefault(reflectionAttribute => reflectionAttribute.FullName == attribute.AttributeType.FullName)))
				.Where(commonAttribute => commonAttribute.ReflectionAttribute != null)
				.ToArray();
		}
	}
}