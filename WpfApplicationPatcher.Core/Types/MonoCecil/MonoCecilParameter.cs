using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilParameter : ObjectBase<ParameterDefinition> {
		public MonoCecilTypeReference ParameterType => Instance.ParameterType.ToMonoCecilTypeReference();

		public MonoCecilParameter(ParameterDefinition instance) : base(instance) {
		}
	}
}