using System;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Types.Attributes.Methods {
	[AttributeUsage(AttributeTargets.Method)]
	public class PatchingToCommandAttribute : Attribute {
		private readonly CommandMethodType commandMethodType;
		private readonly string commandPropertyName;

		public PatchingToCommandAttribute(CommandMethodType commandMethodType, string commandPropertyName = null) {
			this.commandMethodType = commandMethodType;
			this.commandPropertyName = commandPropertyName;
		}
	}
}