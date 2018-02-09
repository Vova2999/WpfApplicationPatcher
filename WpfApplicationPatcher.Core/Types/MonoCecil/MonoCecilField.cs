using Mono.Cecil;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilField : FieldBase<FieldDefinition> {
		public override string Name => GetOrCreate(() => Instance.Name);

		internal MonoCecilField(FieldDefinition instance) : base(instance) {
		}

		public static MonoCecilField Create(string fieldName, FieldAttributes fieldAttributes, MonoCecilTypeReference monoCecilTypeReference) {
			return new MonoCecilField(new FieldDefinition(fieldName, fieldAttributes, monoCecilTypeReference.Instance));
		}
	}
}