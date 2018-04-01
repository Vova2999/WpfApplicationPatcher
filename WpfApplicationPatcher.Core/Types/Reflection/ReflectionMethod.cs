using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionMethod : MethodBase<MethodInfo, ReflectionParameter, ReflectionAttribute> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override string FullName => GetOrCreate(() => Instance.DeclaringType == null ? Name : $"{Instance.DeclaringType?.FullName}.{Name}");
		public override IEnumerable<ReflectionParameter> Parameters => GetOrCreate(() => Instance.GetParameters().ToReflectionParameters().ToArray());
		public override IEnumerable<ReflectionAttribute> Attributes => GetOrCreate(() => Instance.GetCustomAttributes().ToReflectionAttributes());
		public virtual bool IsPublic => GetOrCreate(() => Instance.IsPublic);

		internal ReflectionMethod(MethodInfo instance) : base(instance) {
		}

		public override ReflectionParameter GetParameterByIndex(int index) {
			return Parameters.ToCreatedArray()[index];
		}
	}
}