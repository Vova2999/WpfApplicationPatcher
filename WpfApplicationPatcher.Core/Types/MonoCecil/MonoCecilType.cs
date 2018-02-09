using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilType : TypeBase<TypeDefinition, MonoCecilTypeReference, MonoCecilField, MonoCecilMethod, MonoCecilProperty, MonoCecilAttribute> {
		public override string FullName => GetOrCreate(() => Instance.FullName);
		public override MonoCecilTypeReference BaseType => GetOrCreate(() => Instance.BaseType?.ToMonoCecilTypeReference());
		public override IEnumerable<MonoCecilField> Fields => GetOrCreate(() => Instance.Fields.ToMonoCecilFields());
		public override IEnumerable<MonoCecilMethod> Methods => GetOrCreate(() => Instance.Methods.ToMonoCecilMethods());
		public override IEnumerable<MonoCecilProperty> Properties => GetOrCreate(() => Instance.Properties.ToMonoCecilProperties());
		public override IEnumerable<MonoCecilAttribute> Attributes => GetOrCreate(() => Instance.CustomAttributes.ToMonoCecilAttributes());
		public MonoCecilModule Module => GetOrCreate(() => Instance.Module.ToMonoCecilModule());

		internal MonoCecilType(TypeDefinition instance) : base(instance) {
		}

		public void AddField(MonoCecilField monoCecilField) {
			Instance.Fields.Add(monoCecilField.Instance);
		}
	}
}