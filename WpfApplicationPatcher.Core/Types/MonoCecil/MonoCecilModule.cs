using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilModule : ObjectBase<ModuleDefinition> {
		public MonoCecilModule(ModuleDefinition instance) : base(instance) {
		}
		public IEnumerable<MonoCecilType> Types => Instance.Types.ToMonoCecilTypes();
		public MonoCecilMethodReference Import(MonoCecilGenericInstanceMethod monoCecilGenericInstanceMethod) {
			return Instance.Import(monoCecilGenericInstanceMethod.Instance).ToMonoCecilMethodReference();
		}
	}
}