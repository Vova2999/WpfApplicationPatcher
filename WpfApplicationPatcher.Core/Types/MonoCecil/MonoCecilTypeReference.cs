using Mono.Cecil;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilTypeReference : ObjectBase<TypeReference> {
		public string FullName => GetOrCreate(() => Instance.FullName);
		public bool IsByReference => GetOrCreate(() => Instance.IsByReference);
		public bool IsGenericParameter => GetOrCreate(() => Instance.IsGenericParameter);

		internal MonoCecilTypeReference(TypeReference instance) : base(instance) {
		}

		public MonoCecilType Resolve() {
			return new MonoCecilType(Instance.Resolve());
		}
	}
}