namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class AttributeBase<TAttribute, TType> : ObjectBase<TAttribute> where TAttribute : class {
		public abstract string Name { get; }
		public abstract string FullName { get; }
		public abstract TType AttributeType { get; }

		protected AttributeBase(TAttribute instance) : base(instance) {
		}
	}
}