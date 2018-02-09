using System.Collections.Generic;

namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class MethodBase<TMethod, TParameter, TAttribute> : ObjectBase<TMethod> where TMethod : class {
		public abstract string Name { get; }
		public abstract string FullName { get; }
		public abstract IEnumerable<TParameter> Parameters { get; }
		public abstract IEnumerable<TAttribute> Attributes { get; }

		protected MethodBase(TMethod instance) : base(instance) {
		}

		public abstract TParameter GetParameterByIndex(int index);
	}
}