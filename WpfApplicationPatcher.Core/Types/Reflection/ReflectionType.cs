using System;
using System.Collections.Generic;
using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionType : TypeBase<Type, ReflectionType, ReflectionField, ReflectionMethod, ReflectionProperty, ReflectionAttribute> {
		private const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		public override string FullName => GetOrCreate(() => Instance.FullName);
		public override ReflectionType BaseType => GetOrCreate(() => Instance.BaseType.ToReflectionType());
		public override IEnumerable<ReflectionField> Fields => GetOrCreate(() => Instance.GetFields(bindingFlags).ToReflectionFields());
		public override IEnumerable<ReflectionMethod> Methods => GetOrCreate(() => Instance.GetMethods(bindingFlags).ToReflectionMethods());
		public override IEnumerable<ReflectionProperty> Properties => GetOrCreate(() => Instance.GetProperties(bindingFlags).ToReflectionProperties());
		public override IEnumerable<ReflectionAttribute> Attributes => GetOrCreate(() => Instance.GetCustomAttributes().ToReflectionAttributes());

		internal ReflectionType(Type instance) : base(instance) {
		}
	}
}