using System;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionAttribute : ObjectBase<Attribute> {
		public string FullName => Instance.GetType().FullName;

		public ReflectionAttribute(Attribute instance) : base(instance) {
		}
	}
}