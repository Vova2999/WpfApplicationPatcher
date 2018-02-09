using Mono.Cecil.Cil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilMethodBody : ObjectBase<MethodBody> {
		public MonoCecilMethodBody(MethodBody instance) : base(instance) {
		}
		public MonoCecilInstructions Instructions => Instance.Instructions.ToMonoCecilInstructions();
	}
}