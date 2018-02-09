using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilGenericInstanceMethod : ObjectBase<GenericInstanceMethod> {
		public static MonoCecilGenericInstanceMethod Create(MonoCecilMethod monoCecilMethod) {
			return new MonoCecilGenericInstanceMethod(new GenericInstanceMethod(monoCecilMethod.Instance));
		}
		public MonoCecilGenericInstanceMethod(GenericInstanceMethod instance) : base(instance) {
		}
		public IEnumerable<MonoCecilTypeReference> GenericArguments => Instance.GenericArguments.ToMonoCecilTypeReferences();
		public void AddGenericArgument(MonoCecilTypeReference propertyPropertyType) {
			Instance.GenericArguments.Add(propertyPropertyType.Instance);
		}
		public void AddGenericArgument(MonoCecilType propertyPropertyType) {
			Instance.GenericArguments.Add(propertyPropertyType.Instance);
		}
	}
}