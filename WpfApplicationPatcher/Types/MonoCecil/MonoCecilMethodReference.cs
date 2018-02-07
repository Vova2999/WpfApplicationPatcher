using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilMethodReference : ObjectBase<MethodReference> {
		public string Name => Instance.Name;
		public List<MonoCecilParameter> Parameters => Instance.Parameters.ToMonoCecilParameters().ToList();
		public string FullName => Instance.FullName;

		public MonoCecilMethodReference(MethodReference instance) : base(instance) {
		}
	}
}