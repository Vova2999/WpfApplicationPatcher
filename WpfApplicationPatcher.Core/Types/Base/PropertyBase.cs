using System.Collections.Generic;

namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class PropertyBase<TProperty, TMethod, TType, TAttribute> : ObjectBase<TProperty> where TProperty : class {
		public abstract string Name { get; }
		public abstract string FullName { get; }
		public abstract TMethod GetMethod { get; }
		public abstract TMethod SetMethod { get; }
		public abstract TType PropertyType { get; }
		public abstract IEnumerable<TAttribute> Attributes { get; }

		protected PropertyBase(TProperty instance) : base(instance) {
		}
	}
}