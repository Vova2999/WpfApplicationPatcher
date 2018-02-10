using System.Collections.Generic;

namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class FieldBase<TField, TAttribute> : ObjectBase<TField> where TField : class {
		public abstract string Name { get; }
		public abstract string FullName { get; }
		public abstract IEnumerable<TAttribute> Attributes { get; }

		protected FieldBase(TField instance) : base(instance) {
		}
	}
}