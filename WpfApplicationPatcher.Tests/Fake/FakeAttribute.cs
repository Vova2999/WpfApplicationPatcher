using System;

namespace WpfApplicationPatcher.Tests.Fake {
	public class FakeAttribute {
		public readonly string Name;
		public readonly Type AttributeType;
		public readonly Attribute AttributeInstance;

		public FakeAttribute(Attribute attributeInstance) {
			Name = attributeInstance.GetType().Name;
			AttributeType = attributeInstance.GetType();
			AttributeInstance = attributeInstance;
		}
	}
}