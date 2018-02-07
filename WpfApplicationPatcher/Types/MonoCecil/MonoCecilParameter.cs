using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilParameter : ObjectBase<ParameterDefinition> {
		public MonoCecilTypeReference ParameterType => Instance.ParameterType.ToMonoCecilTypeReference();
		
		public MonoCecilParameter(ParameterDefinition instance) : base(instance) {
		}
	}
}