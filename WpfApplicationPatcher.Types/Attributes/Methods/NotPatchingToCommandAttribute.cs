using System;

namespace WpfApplicationPatcher.Types.Attributes.Methods {
	[AttributeUsage(AttributeTargets.Method)]
	public class NotPatchingToCommandAttribute : Attribute {
	}
}