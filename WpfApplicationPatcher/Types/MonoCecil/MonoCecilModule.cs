using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilModule : ObjectBase<ModuleDefinition> {
		public MonoCecilModule(ModuleDefinition instance) : base(instance) {
		}
		public IEnumerable<MonoCecilType> Types => Instance.Types.ToMonoCecilTypes();
		public MonoCecilMethodReference Import(MonoCecilGenericInstanceMethod monoCecilGenericInstanceMethod) {
			return Instance.Import(monoCecilGenericInstanceMethod.Instance).ToMonoCecilMethodReference();
		}
	}
}