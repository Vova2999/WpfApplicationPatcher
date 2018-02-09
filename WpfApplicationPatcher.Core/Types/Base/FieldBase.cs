namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class FieldBase<TField> : ObjectBase<TField> where TField : class {
		public abstract string Name { get; }

		protected FieldBase(TField instance) : base(instance) {
		}
	}
}