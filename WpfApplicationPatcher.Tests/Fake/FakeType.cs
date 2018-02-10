using System;

namespace WpfApplicationPatcher.Tests.Fake {
	public class FakeType {
		public readonly string Name;
		public readonly string FullName;

		public FakeType(Type type) {
			Name = type.Name;
			FullName = type.FullName;
		}
		public FakeType(string name) {
			Name = name;
			FullName = name;
		}
	}
}