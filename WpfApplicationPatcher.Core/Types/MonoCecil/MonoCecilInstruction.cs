using Mono.Cecil.Cil;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilInstruction : ObjectBase<Instruction> {
		internal MonoCecilInstruction(Instruction instance) : base(instance) {
		}
	}
}