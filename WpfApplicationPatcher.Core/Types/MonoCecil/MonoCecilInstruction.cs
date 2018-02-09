using Mono.Cecil.Cil;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilInstruction : ObjectBase<Instruction> {
		private MonoCecilInstruction(Instruction instance) : base(instance) {
		}

		public static MonoCecilInstruction Create(OpCode opCode) {
			return new MonoCecilInstruction(Instruction.Create(opCode));
		}

		public static MonoCecilInstruction Create(OpCode opCode, string value) {
			return new MonoCecilInstruction(Instruction.Create(opCode, value));
		}

		public static MonoCecilInstruction Create(OpCode opCode, MonoCecilField monoCecilField) {
			return new MonoCecilInstruction(Instruction.Create(opCode, monoCecilField.Instance));
		}

		public static MonoCecilInstruction Create(OpCode opCode, MonoCecilMethodReference monoCecilMethodReference) {
			return new MonoCecilInstruction(Instruction.Create(opCode, monoCecilMethodReference.Instance));
		}
	}
}