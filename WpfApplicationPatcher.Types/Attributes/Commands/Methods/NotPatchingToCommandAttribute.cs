using System;

namespace WpfApplicationPatcher.Types.Attributes.Commands.Methods {
	[AttributeUsage(AttributeTargets.Method)]
	public class NotPatchingToCommandAttribute : Attribute {
	}
}