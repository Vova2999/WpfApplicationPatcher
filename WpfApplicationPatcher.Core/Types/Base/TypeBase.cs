using System.Collections.Generic;

namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class TypeBase<TType, TBaseType, TField, TMethod, TProperty, TAttribute> : ObjectBase<TType> where TType : class {
		public abstract string Name { get; }
		public abstract string FullName { get; }
		public abstract TBaseType BaseType { get; }
		public abstract IEnumerable<TField> Fields { get; }
		public abstract IEnumerable<TMethod> Methods { get; }
		public abstract IEnumerable<TProperty> Properties { get; }
		public abstract IEnumerable<TAttribute> Attributes { get; }

		protected TypeBase(TType instance) : base(instance) {
		}
	}
}