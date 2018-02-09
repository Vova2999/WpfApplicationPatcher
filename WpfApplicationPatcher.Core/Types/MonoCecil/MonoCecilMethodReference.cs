using Mono.Cecil;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilMethodReference : ObjectBase<MethodReference> {
		internal MonoCecilMethodReference(MethodReference instance) : base(instance) {
		}
	}
}