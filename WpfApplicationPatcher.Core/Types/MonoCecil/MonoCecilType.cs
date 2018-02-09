using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilType : ObjectBase<TypeDefinition> {
		public string FullName => Instance.FullName;
		public IEnumerable<MonoCecilMethod> Methods => Instance.Methods.Select(methodDefinition => new MonoCecilMethod(methodDefinition));
		public IEnumerable<MonoCecilProperty> Properties => Instance.Properties.ToMonoCecilProperties();
		public IEnumerable<MonoCecilAttribute> CustomAttributes => Instance.CustomAttributes.ToMonoCecilAttributes();
		public MonoCecilModule Module => Instance.Module.ToMonoCecilModule();
		public MonoCecilTypeReference BaseType => Instance.BaseType?.ToMonoCecilTypeReference();
		public IEnumerable<MonoCecilField> Fields => Instance.Fields.ToMonoCecilFields();
		public bool IsByReference => Instance.IsByReference;
		public bool IsGenericParameter => Instance.IsGenericParameter;

		public MonoCecilType(TypeDefinition instance) : base(instance) {
		}

		public void AddField(MonoCecilField monoCecilField) {
			Instance.Fields.Add(monoCecilField.Instance);
		}
	}
}