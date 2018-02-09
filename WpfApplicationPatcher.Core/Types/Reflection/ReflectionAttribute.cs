using System;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Types.Base;

namespace WpfApplicationPatcher.Core.Types.Reflection {
	public class ReflectionAttribute : AttributeBase<Attribute, ReflectionType> {
		public override string FullName => GetOrCreate(() => AttributeType.FullName);
		public override ReflectionType AttributeType => GetOrCreate(() => Instance.GetType().ToReflectionType());

		internal ReflectionAttribute(Attribute instance) : base(instance) {
		}
	}
}