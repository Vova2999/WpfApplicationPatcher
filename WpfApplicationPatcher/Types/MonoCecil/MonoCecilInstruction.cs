using Mono.Cecil.Cil;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.MonoCecil {
	public class MonoCecilInstruction : ObjectBase<Instruction> {
		public MonoCecilInstruction(Instruction instance) : base(instance) {
		}

		public static MonoCecilInstruction Create(OpCode opCode) {
			return new MonoCecilInstruction(Instruction.Create(opCode));
		}

		internal static MonoCecilInstruction Create(OpCode opCode, MonoCecilField monoCecilField) {
			return new MonoCecilInstruction(Instruction.Create(opCode, monoCecilField.Instance));
		}
		public static MonoCecilInstruction Create(OpCode opCode, string monoCecilField) {
			return new MonoCecilInstruction(Instruction.Create(opCode, monoCecilField));
		}
		public static MonoCecilInstruction Create(OpCode opCode, MonoCecilMethodReference setMethodInViewModelBaseWithGenericParameter) {
			return new MonoCecilInstruction(Instruction.Create(opCode, setMethodInViewModelBaseWithGenericParameter.Instance));
		}
	}
}