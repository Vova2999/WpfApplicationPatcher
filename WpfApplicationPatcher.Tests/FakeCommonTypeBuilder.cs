using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Moq;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;
using WpfApplicationPatcher.Tests.Fake;

namespace WpfApplicationPatcher.Tests {
	public class FakeCommonTypeBuilder {
		private const string fakeNamespace = "FakeNamespace";

		private readonly string typeName;
		private readonly Type baseType;
		private readonly List<FakeField> fields = new List<FakeField>();
		private readonly List<FakeMethod> methods = new List<FakeMethod>();
		private readonly List<FakeProperty> properties = new List<FakeProperty>();
		private readonly List<FakeAttribute> attributes = new List<FakeAttribute>();

		public FakeCommonTypeBuilder(string typeName, Type baseType) {
			this.typeName = typeName;
			this.baseType = baseType;
		}

		public FakeCommonTypeBuilder AddField(FakeField field) {
			fields.Add(field);
			return this;
		}
		public FakeCommonTypeBuilder AddMethod(FakeMethod method) {
			methods.Add(method);
			return this;
		}
		public FakeCommonTypeBuilder AddProperty(FakeProperty property) {
			properties.Add(property);
			return this;
		}
		public FakeCommonTypeBuilder AddAttribute(FakeAttribute attribute) {
			attributes.Add(attribute);
			return this;
		}

		public CommonType Build() {
			var assemblyName = new AssemblyName("DynamicAssembly");
			var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
			var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);
			typeBuilder.SetParent(baseType);
			var currentType = typeBuilder.CreateType();

			var monoCecilType = new Mock<MonoCecilType>(null);
			monoCecilType.Setup(type => type.Name).Returns(() => typeName);
			monoCecilType.Setup(type => type.FullName).Returns(() => CreateFullName(typeName));

			var reflectionType = new Mock<ReflectionType>(currentType);
			reflectionType.Setup(type => type.Name).Returns(() => typeName);
			reflectionType.Setup(type => type.FullName).Returns(() => CreateFullName(typeName));

			var commonType = new Mock<CommonType>(monoCecilType.Object, reflectionType.Object);
			commonType.Setup(type => type.Name).Returns(() => typeName);
			commonType.Setup(type => type.FullName).Returns(() => CreateFullName(typeName));

			CreateFields(commonType, monoCecilType, reflectionType);
			CreateMethods(commonType, monoCecilType, reflectionType);
			CreateProperties(commonType, monoCecilType, reflectionType);
			CreateAttributes(commonType, monoCecilType, reflectionType);

			return commonType.Object;
		}

		private void CreateFields(Mock<CommonType> commonType, Mock<MonoCecilType> monoCecilType, Mock<ReflectionType> reflectionType) {
			var commonFields = (fields?.Select(field => CreateCommonField(commonType, field)) ?? Enumerable.Empty<CommonField>()).ToArray();

			commonType.Setup(type => type.Fields).Returns(() => commonFields);
			monoCecilType.Setup(type => type.Fields).Returns(() => commonFields.Select(field => field.MonoCecilField));
			reflectionType.Setup(type => type.Fields).Returns(() => commonFields.Select(field => field.ReflectionField));
		}
		private static CommonField CreateCommonField(Mock<CommonType> commonType, FakeField fakeField) {
			if (fakeField == null)
				return null;

			var fieldName = fakeField.Name;
			var fieldFullName = $"{commonType.Object.FullName}.{fieldName}";
			var commonAttributes = CreateCommonAttributes(fakeField.Attributes);

			var monoCecilField = new Mock<MonoCecilField>(null);
			monoCecilField.Setup(field => field.Name).Returns(() => fieldName);
			monoCecilField.Setup(field => field.FullName).Returns(() => fieldFullName);
			monoCecilField.Setup(field => field.Attributes).Returns(() => commonAttributes.Select(attribute => attribute.MonoCecilAttribute));

			var reflectionField = new Mock<ReflectionField>(null);
			reflectionField.Setup(field => field.Name).Returns(() => fieldName);
			reflectionField.Setup(field => field.FullName).Returns(() => fieldFullName);
			reflectionField.Setup(field => field.Attributes).Returns(() => commonAttributes.Select(attribute => attribute.ReflectionAttribute));

			return new Mock<CommonField>(commonAttributes, monoCecilField.Object, reflectionField.Object).Object;
		}

		private void CreateMethods(Mock<CommonType> commonType, Mock<MonoCecilType> monoCecilType, Mock<ReflectionType> reflectionType) {
			var commonMethods = (methods?.Select(method => CreateCommonMethod(commonType, method)) ?? Enumerable.Empty<CommonMethod>()).ToArray();

			commonType.Setup(type => type.Methods).Returns(() => commonMethods);
			monoCecilType.Setup(type => type.Methods).Returns(() => commonMethods.Select(field => field.MonoCecilMethod));
			reflectionType.Setup(type => type.Methods).Returns(() => commonMethods.Select(field => field.ReflectionMethod));
		}
		private static CommonMethod CreateCommonMethod(Mock<CommonType> commonType, FakeMethod fakeMethod) {
			if (fakeMethod == null)
				return null;

			var fieldName = fakeMethod.Name;
			var fieldFullName = $"{commonType.Object.FullName}.{fieldName}";
			var commonParameters = CreateCommonParameters(fakeMethod.Parameters);
			var commonAttributes = CreateCommonAttributes(fakeMethod.Attributes);

			var monoCecilMethod = new Mock<MonoCecilMethod>(null);
			monoCecilMethod.Setup(method => method.Name).Returns(() => fieldName);
			monoCecilMethod.Setup(method => method.FullName).Returns(() => fieldFullName);
			monoCecilMethod.Setup(method => method.Parameters).Returns(() => commonParameters.Select(field => field.MonoCecilParameter));
			monoCecilMethod.Setup(method => method.Attributes).Returns(() => commonAttributes.Select(field => field.MonoCecilAttribute));
			monoCecilMethod.Setup(method => method.GetParameterByIndex(It.IsAny<int>())).Returns((int index) => commonParameters[index].MonoCecilParameter);

			var monoCecilMethodBody = new Mock<MonoCecilMethodBody>(null);
			var monoCecilInstructions = new Mock<MonoCecilInstructions>(null);
			monoCecilMethodBody.Setup(body => body.Instructions).Returns(() => monoCecilInstructions.Object);
			monoCecilMethod.Setup(method => method.Body).Returns(() => monoCecilMethodBody.Object);

			var reflectionMethod = new Mock<ReflectionMethod>(null);
			reflectionMethod.Setup(method => method.Name).Returns(() => fieldName);
			reflectionMethod.Setup(method => method.FullName).Returns(() => fieldFullName);
			reflectionMethod.Setup(method => method.Parameters).Returns(() => commonParameters.Select(field => field.ReflectionParameter));
			reflectionMethod.Setup(method => method.Attributes).Returns(() => commonAttributes.Select(field => field.ReflectionAttribute));
			reflectionMethod.Setup(method => method.GetParameterByIndex(It.IsAny<int>())).Returns((int index) => commonParameters[index].ReflectionParameter);

			return new Mock<CommonMethod>(commonAttributes, monoCecilMethod.Object, reflectionMethod.Object).Object;
		}
		private static CommonParameter[] CreateCommonParameters(IEnumerable<FakeParameter> fakeParameters) {
			return fakeParameters?.Select(CreateCommonParameter).ToArray();
		}
		private static CommonParameter CreateCommonParameter(FakeParameter fakeParameter) {
			if (fakeParameter == null)
				return null;

			var monoCecilParameter = new Mock<MonoCecilParameter>(null);
			monoCecilParameter.Setup(parameter => parameter.Name).Returns(() => fakeParameter.Name);
			monoCecilParameter.Setup(parameter => parameter.ParameterType).Returns(() => CreateMonoCecilTypeReference(fakeParameter.ParameterType));

			var reflectionParameter = new Mock<ReflectionParameter>(null);
			reflectionParameter.Setup(parameter => parameter.Name).Returns(() => fakeParameter.Name);
			reflectionParameter.Setup(parameter => parameter.ParameterType).Returns(() => CreateReflectionType(fakeParameter.ParameterType));

			return new Mock<CommonParameter>(monoCecilParameter.Object, reflectionParameter.Object).Object;
		}

		private void CreateProperties(Mock<CommonType> commonType, Mock<MonoCecilType> monoCecilType, Mock<ReflectionType> reflectionType) {
			var commonProperties = (properties?.Select(property => CreateCommonProperty(commonType, property)) ?? Enumerable.Empty<CommonProperty>()).ToArray();

			commonType.Setup(type => type.Properties).Returns(() => commonProperties);
			monoCecilType.Setup(type => type.Properties).Returns(() => commonProperties.Select(field => field.MonoCecilProperty));
			reflectionType.Setup(type => type.Properties).Returns(() => commonProperties.Select(field => field.ReflectionProperty));
		}
		private static CommonProperty CreateCommonProperty(Mock<CommonType> commonType, FakeProperty fakeProperty) {
			if (fakeProperty == null)
				return null;

			var propertyName = fakeProperty.Name;
			var propertyFullName = CreateFullName(propertyName);

			if (fakeProperty.GetMethod != null)
				fakeProperty.GetMethod.Name = $"get_{propertyName}";
			if (fakeProperty.SetMethod != null)
				fakeProperty.SetMethod.Name = $"set_{propertyName}";

			var commonAttributes = CreateCommonAttributes(fakeProperty.Attributes);
			var propertyGetMethod = CreateCommonMethod(commonType, fakeProperty.GetMethod);
			var propertySetMethod = CreateCommonMethod(commonType, fakeProperty.SetMethod);

			var monoCecilProperty = new Mock<MonoCecilProperty>(null);
			monoCecilProperty.Setup(property => property.Name).Returns(() => propertyName);
			monoCecilProperty.Setup(property => property.FullName).Returns(() => propertyFullName);
			monoCecilProperty.Setup(property => property.GetMethod).Returns(() => propertyGetMethod?.MonoCecilMethod);
			monoCecilProperty.Setup(property => property.SetMethod).Returns(() => propertySetMethod?.MonoCecilMethod);
			monoCecilProperty.Setup(property => property.PropertyType).Returns(() => CreateMonoCecilTypeReference(fakeProperty.PropertyType));
			monoCecilProperty.Setup(property => property.Attributes).Returns(() => commonAttributes.Select(attribute => attribute.MonoCecilAttribute));

			var reflectionProperty = new Mock<ReflectionProperty>(null);
			reflectionProperty.Setup(property => property.Name).Returns(() => propertyName);
			reflectionProperty.Setup(property => property.FullName).Returns(() => propertyFullName);
			reflectionProperty.Setup(property => property.GetMethod).Returns(() => propertyGetMethod?.ReflectionMethod);
			reflectionProperty.Setup(property => property.SetMethod).Returns(() => propertySetMethod?.ReflectionMethod);
			reflectionProperty.Setup(property => property.PropertyType).Returns(() => CreateReflectionType(fakeProperty.PropertyType));
			reflectionProperty.Setup(property => property.Attributes).Returns(() => commonAttributes.Select(attribute => attribute.ReflectionAttribute));

			return new Mock<CommonProperty>(commonAttributes, monoCecilProperty.Object, reflectionProperty.Object).Object;
		}

		private void CreateAttributes(Mock<CommonType> commonType, Mock<MonoCecilType> monoCecilType, Mock<ReflectionType> reflectionType) {
			var commonAttributes = CreateCommonAttributes(attributes);

			commonType.Setup(type => type.Attributes).Returns(() => commonAttributes);
			monoCecilType.Setup(type => type.Attributes).Returns(() => commonAttributes.Select(field => field.MonoCecilAttribute));
			reflectionType.Setup(type => type.Attributes).Returns(() => commonAttributes.Select(field => field.ReflectionAttribute));
		}
		private static CommonAttribute[] CreateCommonAttributes(IEnumerable<FakeAttribute> fakeAttributes) {
			return (fakeAttributes?.Select(CreateCommonAttribute) ?? Enumerable.Empty<CommonAttribute>()).ToArray();
		}
		private static CommonAttribute CreateCommonAttribute(FakeAttribute fakeAttribute) {
			if (fakeAttribute == null)
				return null;

			var attributeName = fakeAttribute.Name;
			var attributeFullName = CreateFullName(attributeName);

			var monoCecilAttribute = new Mock<MonoCecilAttribute>(null);
			monoCecilAttribute.Setup(attribute => attribute.Name).Returns(() => attributeName);
			monoCecilAttribute.Setup(attribute => attribute.FullName).Returns(() => attributeFullName);
			monoCecilAttribute.Setup(attribute => attribute.AttributeType).Returns(() => CreateMonoCecilTypeReference(fakeAttribute.AttributeType));

			var reflectionAttribute = new Mock<ReflectionAttribute>(null);
			reflectionAttribute.Setup(attribute => attribute.Name).Returns(() => attributeName);
			reflectionAttribute.Setup(attribute => attribute.FullName).Returns(() => attributeFullName);
			reflectionAttribute.Setup(attribute => attribute.AttributeType).Returns(() => CreateReflectionType(fakeAttribute.AttributeType));

			return new Mock<CommonAttribute>(monoCecilAttribute.Object, reflectionAttribute.Object).Object;
		}

		private static string CreateFullName(string name) {
			return $"{fakeNamespace}.{name}";
		}
		private static MonoCecilTypeReference CreateMonoCecilTypeReference(FakeType fakeType) {
			if (fakeType == null)
				return null;

			var monoCecilTypeReference = new Mock<MonoCecilTypeReference>(null);
			monoCecilTypeReference.Setup(reference => reference.Name).Returns(() => fakeType.Name);
			monoCecilTypeReference.Setup(reference => reference.FullName).Returns(() => fakeType.FullName);

			return monoCecilTypeReference.Object;
		}
		private static ReflectionType CreateReflectionType(FakeType fakeType) {
			if (fakeType == null)
				return null;

			var reflectionType = new Mock<ReflectionType>(null);
			reflectionType.Setup(reference => reference.Name).Returns(() => fakeType.Name);
			reflectionType.Setup(reference => reference.FullName).Returns(() => fakeType.FullName);

			return reflectionType.Object;
		}
	}
}