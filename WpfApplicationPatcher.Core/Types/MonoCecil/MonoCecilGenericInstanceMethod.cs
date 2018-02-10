using Mono.Cecil;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilGenericInstanceMethod : ObjectBase<GenericInstanceMethod> {
		internal MonoCecilGenericInstanceMethod(GenericInstanceMethod instance) : base(instance) {
		}

		public virtual void AddGenericArgument(MonoCecilTypeReference monoCecilTypeReference) {
			Instance.GenericArguments.Add(monoCecilTypeReference.Instance);
		}
	}
}