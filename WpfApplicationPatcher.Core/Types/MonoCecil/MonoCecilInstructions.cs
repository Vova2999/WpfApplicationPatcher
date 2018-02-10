using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilInstructions : ObjectBase<Collection<Instruction>> {
		internal MonoCecilInstructions(Collection<Instruction> instance) : base(instance) {
		}

		public virtual void Clear() {
			Instance.Clear();
		}

		public virtual void Add(MonoCecilInstruction instruction) {
			Instance.Add(instruction.Instance);
		}
	}
}