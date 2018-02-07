using System;
using System.Collections.Generic;
using System.Reflection;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.Reflection {
	public class ReflectionProperty : ObjectBase<PropertyInfo> {
		public ReflectionProperty(PropertyInfo instance) : base(instance) {
		}
		public string Name => Instance.Name;
		public Type PropertyType => Instance.PropertyType;
		public IEnumerable<ReflectionAttribute> GetCustomAttributes() {
			return Instance.GetCustomAttributes().ToReflectionAttributes();
		}
	}
}