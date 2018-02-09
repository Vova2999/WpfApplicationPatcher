using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilProperty : ObjectBase<PropertyDefinition> {
		public MonoCecilProperty(PropertyDefinition instance) : base(instance) {
		}
		public IEnumerable<MonoCecilAttribute> CustomAttributes => Instance.CustomAttributes.ToMonoCecilAttributes();
		public string Name => Instance.Name;
		public string FullName => Instance.FullName;
		public MonoCecilTypeReference PropertyType => Instance.PropertyType.ToMonoCecilTypeReference();
		public MonoCecilMethod GetMethod => Instance.GetMethod.ToMonoCecilMethod();
		public MonoCecilMethod SetMethod => Instance.SetMethod.ToMonoCecilMethod();
	}
}