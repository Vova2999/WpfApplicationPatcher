using Mono.Cecil;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilField : ObjectBase<FieldDefinition> {
		public MonoCecilField(FieldDefinition instance) : base(instance) {
		}
		public string Name => Instance.Name;
		public static MonoCecilField Create(string fieldName, FieldAttributes fieldAttributes, MonoCecilTypeReference monoCecilType) {
			return new MonoCecilField(new FieldDefinition(fieldName, fieldAttributes, monoCecilType.Instance));
		}
	}
}