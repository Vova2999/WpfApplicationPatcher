using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilAttribute : ObjectBase<CustomAttribute> {
		public MonoCecilAttribute(CustomAttribute instance) : base(instance) {
		}
		public MonoCecilTypeReference AttributeType => Instance.AttributeType.ToMonoCecilTypeReference();
	}
}