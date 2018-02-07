using System;
using WpfApplicationPatcher.Types.Base;

namespace WpfApplicationPatcher.Types.Reflection {
	public class ReflectionAttribute : ObjectBase<Attribute> {
		public ReflectionAttribute(Attribute instance) : base(instance) {
		}
	}
}