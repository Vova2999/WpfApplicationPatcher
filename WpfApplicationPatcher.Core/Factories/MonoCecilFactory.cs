using Mono.Cecil;
using Mono.Cecil.Cil;
using WpfApplicationPatcher.Core.Types.MonoCecil;

namespace WpfApplicationPatcher.Core.Factories {
	public class MonoCecilFactory {
		public virtual MonoCecilField CreateField(string fieldName, FieldAttributes fieldAttributes, MonoCecilTypeReference monoCecilTypeReference) {
			return new MonoCecilField(new FieldDefinition(fieldName, fieldAttributes, monoCecilTypeReference.Instance));
		}

		public virtual MonoCecilGenericInstanceMethod CreateGenericInstanceMethod(MonoCecilMethod monoCecilMethod) {
			return new MonoCecilGenericInstanceMethod(new GenericInstanceMethod(monoCecilMethod.Instance));
		}

		public virtual MonoCecilInstruction CreateInstruction(OpCode opCode) {
			return new MonoCecilInstruction(Instruction.Create(opCode));
		}
		public virtual MonoCecilInstruction CreateInstruction(OpCode opCode, string value) {
			return new MonoCecilInstruction(Instruction.Create(opCode, value));
		}
		public virtual MonoCecilInstruction CreateInstruction(OpCode opCode, MonoCecilField monoCecilField) {
			return new MonoCecilInstruction(Instruction.Create(opCode, monoCecilField.Instance));
		}
		public virtual MonoCecilInstruction CreateInstruction(OpCode opCode, MonoCecilMethodReference monoCecilMethodReference) {
			return new MonoCecilInstruction(Instruction.Create(opCode, monoCecilMethodReference.Instance));
		}
	}
}