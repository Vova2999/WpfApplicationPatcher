using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilAttribute : AttributeBase<CustomAttribute, MonoCecilTypeReference> {
		public override string FullName => GetOrCreate(() => AttributeType.FullName);
		public override MonoCecilTypeReference AttributeType => GetOrCreate(() => Instance.AttributeType.ToMonoCecilTypeReference());

		internal MonoCecilAttribute(CustomAttribute instance) : base(instance) {
		}
	}
}