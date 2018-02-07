using Mono.Cecil;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilField : ObjectBase<FieldDefinition> {
		public MonoCecilField(FieldDefinition instance) : base(instance) {
		}
		public string Name => Instance.Name;
		public static MonoCecilField Create(string fieldName, FieldAttributes fieldAttributes, MonoCecilTypeReference monoCecilType) {
			return new MonoCecilField(new FieldDefinition(fieldName, fieldAttributes, monoCecilType.Instance));
		}
	}
}