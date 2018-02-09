using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilAssembly : AssemblyBase<AssemblyDefinition, MonoCecilType> {
		public MonoCecilModule MainModule => GetOrCreate(() => Instance.MainModule.ToMonoCecilModule());

		internal MonoCecilAssembly(AssemblyDefinition instance) : base(instance) {
		}

		public override MonoCecilType GetTypeByName(string typeFullName) {
			return MainModule.Types.FirstOrDefault(monoCecilType => monoCecilType.FullName == typeFullName);
		}
	}
}