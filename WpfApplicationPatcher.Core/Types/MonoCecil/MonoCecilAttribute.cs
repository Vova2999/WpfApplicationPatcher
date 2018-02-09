using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilAttribute : ObjectBase<CustomAttribute> {
		public MonoCecilAttribute(CustomAttribute instance) : base(instance) {
		}
		public MonoCecilTypeReference AttributeType => Instance.AttributeType.ToMonoCecilTypeReference();
	}
}