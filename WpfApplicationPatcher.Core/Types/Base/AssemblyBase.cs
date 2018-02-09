using JetBrains.Annotations;

namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class AssemblyBase<TAssembly, TType> : ObjectBase<TAssembly> where TAssembly : class {
		protected AssemblyBase(TAssembly instance) : base(instance) {
		}

		[CanBeNull]
		public abstract TType GetTypeByName(string typeFullName);
	}
}