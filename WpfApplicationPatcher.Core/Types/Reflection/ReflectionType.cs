using System;
using System.Collections.Generic;
using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionType : ObjectBase<Type> {
		public string FullName => Instance.FullName;

		public ReflectionType(Type instance) : base(instance) {
		}

		public IEnumerable<ReflectionMethod> GetMethods(BindingFlags bindingFlags) {
			return Instance.GetMethods(bindingFlags).ToReflectionMethods();
		}
		public IEnumerable<ReflectionProperty> GetProperties(BindingFlags bindingAttr) {
			return Instance.GetProperties(bindingAttr).ToReflectionProperties();
		}
		public IEnumerable<ReflectionAttribute> GetCustomAttributes() {
			return Instance.GetCustomAttributes().ToReflectionAttributes();
		}
		public bool IsAssignableFrom(ReflectionType reflectionType) {
			return Instance.IsAssignableFrom(reflectionType.Instance);
		}
		public TAttribute GetReflectionAttribute<TAttribute>() where TAttribute : Attribute {
			return Instance.GetCustomAttribute<TAttribute>();
		}
	}
}