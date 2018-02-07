using System.Collections.Generic;
using System.Reflection;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;
using WpfApplicationPatcher.Types.MonoCecil;

namespace WpfApplicationPatcher.Types.Reflection {
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