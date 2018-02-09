using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using WpfApplicationPatcher.Core.Types.MonoCecil;

namespace WpfApplicationPatcher.Core.Extensions {
	public static class ToMonoCecilObjectExtensions {
		// ReSharper disable MemberCanBePrivate.Global
		// ReSharper disable UnusedMember.Global

		public static MonoCecilAssembly ToMonoCecilAssembly(this AssemblyDefinition assemblyDefinition) {
			return new MonoCecilAssembly(assemblyDefinition);
		}

		public static IEnumerable<MonoCecilAttribute> ToMonoCecilAttributes(this IEnumerable<CustomAttribute> customAttributes) {
			return customAttributes.Select(customAttribute => customAttribute.ToMonoCecilAttribute());
		}
		public static MonoCecilAttribute ToMonoCecilAttribute(this CustomAttribute customAttribute) {
			return new MonoCecilAttribute(customAttribute);
		}

		public static IEnumerable<MonoCecilField> ToMonoCecilFields(this IEnumerable<FieldDefinition> fieldDefinitions) {
			return fieldDefinitions.Select(fieldDefinition => fieldDefinition.ToMonoCecilField());
		}
		public static MonoCecilField ToMonoCecilField(this FieldDefinition fieldDefinition) {
			return new MonoCecilField(fieldDefinition);
		}

		public static MonoCecilInstructions ToMonoCecilInstructions(this Collection<Instruction> instructions) {
			return new MonoCecilInstructions(instructions);
		}

		public static IEnumerable<MonoCecilMethod> ToMonoCecilMethods(this IEnumerable<MethodDefinition> methodDefinitions) {
			return methodDefinitions.Select(methodDefinition => methodDefinition.ToMonoCecilMethod());
		}
		public static MonoCecilMethod ToMonoCecilMethod(this MethodDefinition methodDefinition) {
			return new MonoCecilMethod(methodDefinition);
		}

		public static IEnumerable<MonoCecilMethodReference> ToMonoCecilMethodReferences(this IEnumerable<MethodReference> methodReferences) {
			return methodReferences.Select(methodReference => methodReference.ToMonoCecilMethodReference());
		}
		public static MonoCecilMethodReference ToMonoCecilMethodReference(this MethodReference methodReference) {
			return new MonoCecilMethodReference(methodReference);
		}

		public static MonoCecilMethodBody ToMonoCecilMethodBody(this MethodBody methodBody) {
			return new MonoCecilMethodBody(methodBody);
		}

		public static MonoCecilModule ToMonoCecilModule(this ModuleDefinition moduleDefinition) {
			return new MonoCecilModule(moduleDefinition);
		}

		public static IEnumerable<MonoCecilParameter> ToMonoCecilParameters(this IEnumerable<ParameterDefinition> parameterDefinitions) {
			return parameterDefinitions.Select(parameterDefinition => parameterDefinition.ToMonoCecilParameter());
		}
		public static MonoCecilParameter ToMonoCecilParameter(this ParameterDefinition parameterDefinition) {
			return new MonoCecilParameter(parameterDefinition);
		}

		public static IEnumerable<MonoCecilProperty> ToMonoCecilProperties(this IEnumerable<PropertyDefinition> propertyDefinitions) {
			return propertyDefinitions.Select(propertyDefinition => propertyDefinition.ToMonoCecilProperty());
		}
		public static MonoCecilProperty ToMonoCecilProperty(this PropertyDefinition propertyDefinition) {
			return new MonoCecilProperty(propertyDefinition);
		}

		public static IEnumerable<MonoCecilType> ToMonoCecilTypes(this IEnumerable<TypeDefinition> typeDefinitions) {
			return typeDefinitions.Select(typeDefinition => typeDefinition.ToMonoCecilType());
		}
		public static MonoCecilType ToMonoCecilType(this TypeDefinition typeDefinition) {
			return new MonoCecilType(typeDefinition);
		}

		public static IEnumerable<MonoCecilTypeReference> ToMonoCecilTypeReferences(this IEnumerable<TypeReference> typeReferences) {
			return typeReferences.Select(typeReference => typeReference.ToMonoCecilTypeReference());
		}
		public static MonoCecilTypeReference ToMonoCecilTypeReference(this TypeReference typeReference) {
			return new MonoCecilTypeReference(typeReference);
		}
	}
}