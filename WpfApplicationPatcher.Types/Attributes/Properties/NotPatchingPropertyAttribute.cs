using System;

namespace WpfApplicationPatcher.Types.Attributes.Properties {
	[AttributeUsage(AttributeTargets.Property)]
	public class NotPatchingPropertyAttribute : Attribute {
	}
}