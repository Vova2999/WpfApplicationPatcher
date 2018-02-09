using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Core.Extensions {
	// ReSharper disable MemberCanBePrivate.Global
	// ReSharper disable UnusedMember.Global

	public static class ToReflectionObjectExtensions {
		public static ReflectionAssembly ToReflectionAssembly(this IEnumerable<Assembly> assemblies) {
			return new ReflectionAssembly(assemblies.ToCreatedArray());
		}

		public static IEnumerable<ReflectionAttribute> ToReflectionAttributes(this IEnumerable<Attribute> attributes) {
			return attributes.Select(attribute => attribute.ToReflectionAttribute());
		}
		public static ReflectionAttribute ToReflectionAttribute(this Attribute attribute) {
			return new ReflectionAttribute(attribute);
		}

		public static IEnumerable<ReflectionField> ToReflectionFields(this IEnumerable<FieldInfo> fieldInfos) {
			return fieldInfos.Select(fieldInfo => fieldInfo.ToReflectionField());
		}
		public static ReflectionField ToReflectionField(this FieldInfo fieldInfo) {
			return new ReflectionField(fieldInfo);
		}

		public static IEnumerable<ReflectionMethod> ToReflectionMethods(this IEnumerable<MethodInfo> methodInfos) {
			return methodInfos.Select(methodInfo => methodInfo.ToReflectionMethod());
		}
		public static ReflectionMethod ToReflectionMethod(this MethodInfo methodInfo) {
			return new ReflectionMethod(methodInfo);
		}

		public static IEnumerable<ReflectionParameter> ToReflectionParameters(this IEnumerable<ParameterInfo> parameterInfos) {
			return parameterInfos.Select(parameterInfo => parameterInfo.ToReflectionParameter());
		}
		public static ReflectionParameter ToReflectionParameter(this ParameterInfo parameterInfo) {
			return new ReflectionParameter(parameterInfo);
		}

		public static IEnumerable<ReflectionProperty> ToReflectionProperties(this IEnumerable<PropertyInfo> propertyInfos) {
			return propertyInfos.Select(propertyInfo => propertyInfo.ToReflectionProperty());
		}
		public static ReflectionProperty ToReflectionProperty(this PropertyInfo propertyInfo) {
			return new ReflectionProperty(propertyInfo);
		}

		public static IEnumerable<ReflectionType> ToReflectionTypes(this IEnumerable<Type> types) {
			return types.Select(type => type.ToReflectionType());
		}
		public static ReflectionType ToReflectionType(this Type type) {
			return new ReflectionType(type);
		}
	}
}