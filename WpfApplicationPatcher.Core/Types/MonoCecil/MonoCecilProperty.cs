using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilProperty : PropertyBase<PropertyDefinition, MonoCecilMethod, MonoCecilTypeReference, MonoCecilAttribute> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override string FullName => GetOrCreate(() => Instance.FullName);
		public override MonoCecilMethod GetMethod => GetOrCreate(() => Instance.GetMethod.ToMonoCecilMethod());
		public override MonoCecilMethod SetMethod => GetOrCreate(() => Instance.SetMethod.ToMonoCecilMethod());
		public override MonoCecilTypeReference PropertyType => GetOrCreate(() => Instance.PropertyType.ToMonoCecilTypeReference());
		public override IEnumerable<MonoCecilAttribute> Attributes => GetOrCreate(() => Instance.CustomAttributes.ToMonoCecilAttributes());

		internal MonoCecilProperty(PropertyDefinition instance) : base(instance) {
		}
	}
}