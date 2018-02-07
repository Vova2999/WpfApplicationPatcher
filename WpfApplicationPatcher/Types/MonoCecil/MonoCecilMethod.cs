using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilMethod : ObjectBase<MethodDefinition> {
		public string Name => Instance.Name;
		public List<MonoCecilParameter> Parameters => Instance.Parameters.ToMonoCecilParameters().ToList();
		public IEnumerable<MonoCecilAttribute> CustomAttributes => Instance.CustomAttributes.ToMonoCecilAttributes();
		public string FullName => Instance.FullName;
		public MonoCecilMethodBody Body => Instance.Body.ToMonoCecilMethodBody();

		public MonoCecilMethod(MethodDefinition instance) : base(instance) {
		}
	}
}