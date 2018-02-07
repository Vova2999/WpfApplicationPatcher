using Mono.Cecil.Cil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilMethodBody : ObjectBase<MethodBody> {
		public MonoCecilMethodBody(MethodBody instance) : base(instance) {
		}
		public MonoCecilInstructions Instructions => Instance.Instructions.ToMonoCecilInstructions();
	}
}