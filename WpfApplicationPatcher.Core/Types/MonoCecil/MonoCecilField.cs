using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilField : FieldBase<FieldDefinition, MonoCecilAttribute> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override string FullName => GetOrCreate(() => Instance.FullName);
		public override IEnumerable<MonoCecilAttribute> Attributes => GetOrCreate(() => Instance.CustomAttributes.ToMonoCecilAttributes());

		internal MonoCecilField(FieldDefinition instance) : base(instance) {
		}
	}
}