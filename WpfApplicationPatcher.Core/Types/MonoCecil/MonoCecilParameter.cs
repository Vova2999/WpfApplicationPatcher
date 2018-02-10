using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilParameter : ParameterBase<ParameterDefinition, MonoCecilTypeReference> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override MonoCecilTypeReference ParameterType => GetOrCreate(() => Instance.ParameterType.ToMonoCecilTypeReference());

		internal MonoCecilParameter(ParameterDefinition instance) : base(instance) {
		}
	}
}