using Mono.Cecil;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilTypeReference : ObjectBase<TypeReference> {
		public virtual string Name => GetOrCreate(() => Instance.Name);
		public virtual string FullName => GetOrCreate(() => Instance.FullName);

		internal MonoCecilTypeReference(TypeReference instance) : base(instance) {
		}

		public virtual MonoCecilType Resolve() {
			return new MonoCecilType(Instance.Resolve());
		}
	}
}