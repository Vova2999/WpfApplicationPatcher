using Mono.Cecil.Cil;
using WpfApplicationPatcher.Core.Extensions;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilMethodBody : ObjectBase<MethodBody> {
		public MonoCecilInstructions Instructions => GetOrCreate(() => Instance.Instructions.ToMonoCecilInstructions());

		internal MonoCecilMethodBody(MethodBody instance) : base(instance) {
		}
	}
}