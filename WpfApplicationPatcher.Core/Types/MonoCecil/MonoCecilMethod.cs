using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilMethod : MethodBase<MethodDefinition, MonoCecilParameter, MonoCecilAttribute> {
		public override string Name => GetOrCreate(() => Instance.Name);
		public override string FullName => GetOrCreate(() => Instance.FullName);
		public override IEnumerable<MonoCecilParameter> Parameters => GetOrCreate(() => Instance.Parameters.ToMonoCecilParameters());
		public override IEnumerable<MonoCecilAttribute> Attributes => GetOrCreate(() => Instance.CustomAttributes.ToMonoCecilAttributes());
		public virtual MonoCecilMethodBody Body => GetOrCreate(() => Instance.Body.ToMonoCecilMethodBody());

		internal MonoCecilMethod(MethodDefinition instance) : base(instance) {
		}

		public override MonoCecilParameter GetParameterByIndex(int index) {
			return Instance.Parameters[index].ToMonoCecilParameter();
		}

		public virtual void RemoveAttribute(string attributeFullName) {
			var removedAttribute = Instance.CustomAttributes.FirstOrDefault(attribute => attribute.AttributeType.FullName == attributeFullName);
			if (removedAttribute != null)
				Instance.CustomAttributes.Remove(removedAttribute);
		}
	}
}