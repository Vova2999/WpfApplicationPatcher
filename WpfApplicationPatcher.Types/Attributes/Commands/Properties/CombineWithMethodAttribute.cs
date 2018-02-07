using System;

namespace WpfApplicationPatcher.Types.Attributes.Commands.Properties {
	[AttributeUsage(AttributeTargets.Property)]
	public class CombineWithMethodAttribute : Attribute {
		private readonly string executeMethodName;
		private readonly string canExecuteMethodName;

		public CombineWithMethodAttribute(string executeMethodName = null, string canExecuteMethodName = null) {
			this.executeMethodName = executeMethodName;
			this.canExecuteMethodName = canExecuteMethodName;
		}
	}
}