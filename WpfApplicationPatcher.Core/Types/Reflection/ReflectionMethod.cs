using System.Collections.Generic;
using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionMethod : ObjectBase<MethodInfo> {
		public string Name => Instance.Name;

		public ReflectionMethod(MethodInfo instance) : base(instance) {
		}

		public IEnumerable<ReflectionParameter> GetParameters() {
			return Instance.GetParameters().ToReflectionParameters();
		}

		public IEnumerable<ReflectionAttribute> GetCustomAttributes() {
			return Instance.GetCustomAttributes().ToReflectionAttributes();
		}
	}
}