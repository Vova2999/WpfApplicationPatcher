using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilTypeReference : ObjectBase<TypeReference> {
		public string FullName => Instance.FullName;
		public MonoCecilModule Module => Instance.Module.ToMonoCecilModule();
		public bool IsByReference => Instance.IsByReference;
		public bool IsGenericParameter => Instance.IsGenericParameter;

		public MonoCecilTypeReference(TypeReference instance) : base(instance) {
		}

		public MonoCecilType ToMonoCecilType() {
			return new MonoCecilType(Instance.Resolve());
		}
	}
}