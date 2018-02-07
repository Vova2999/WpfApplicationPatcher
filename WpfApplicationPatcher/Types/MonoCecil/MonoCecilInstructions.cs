using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilInstructions : ObjectBase<Collection<Instruction>> {
		public MonoCecilInstructions(Collection<Instruction> instance) : base(instance) {
		}
		public void Clear() {
			Instance.Clear();
		}
		public void Add(MonoCecilInstruction instruction) {
			Instance.Add(instruction.Instance);
		}
	}
}