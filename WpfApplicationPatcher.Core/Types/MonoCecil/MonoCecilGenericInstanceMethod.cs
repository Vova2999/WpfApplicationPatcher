using Mono.Cecil;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilGenericInstanceMethod : ObjectBase<GenericInstanceMethod> {
		private MonoCecilGenericInstanceMethod(GenericInstanceMethod instance) : base(instance) {
		}

		public static MonoCecilGenericInstanceMethod Create(MonoCecilMethod monoCecilMethod) {
			return new MonoCecilGenericInstanceMethod(new GenericInstanceMethod(monoCecilMethod.Instance));
		}

		public void AddGenericArgument(MonoCecilTypeReference monoCecilTypeReference) {
			Instance.GenericArguments.Add(monoCecilTypeReference.Instance);
		}
	}
}