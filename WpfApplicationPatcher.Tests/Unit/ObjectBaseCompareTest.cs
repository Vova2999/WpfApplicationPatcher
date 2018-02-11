using FluentAssertions;
using Moq;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Tests.Unit {
	// ReSharper disable ConditionIsAlwaysTrueOrFalse
	// ReSharper disable EqualExpressionComparison
	// ReSharper disable RedundantCast

	[TestFixture]
	public class ObjectBaseCompareTest {
		[Test]
		public void EqualityCompareTest() {
			var first = new Mock<ReflectionType>(typeof(ObjectBaseCompareTest)).Object;
			var second = new Mock<ReflectionType>(typeof(ObjectBaseCompareTest)).Object;
			var firstWithNull = new Mock<ReflectionType>(null).Object;
			var secondWithNull = new Mock<ReflectionType>(null).Object;

			(null as ReflectionType == null as ReflectionType).Should().Be(true);
			(first == null as ReflectionType).Should().Be(false);
			(null as ReflectionType == second).Should().Be(false);
			(first == second).Should().Be(true);

			(firstWithNull == null as ReflectionType).Should().Be(false);
			(null as ReflectionType == secondWithNull).Should().Be(false);
			(firstWithNull == secondWithNull).Should().Be(true);

			(firstWithNull == second).Should().Be(false);
			(first == secondWithNull).Should().Be(false);
		}

		[Test]
		public void InequalityCompareTest() {
			var first = new Mock<ReflectionType>(typeof(ObjectBaseCompareTest)).Object;
			var second = new Mock<ReflectionType>(typeof(ObjectBaseCompareTest)).Object;
			var firstWithNull = new Mock<ReflectionType>(null).Object;
			var secondWithNull = new Mock<ReflectionType>(null).Object;

			(null as ReflectionType != null as ReflectionType).Should().Be(false);
			(first != null as ReflectionType).Should().Be(true);
			(null as ReflectionType != second).Should().Be(true);
			(first != second).Should().Be(false);

			(firstWithNull != null as ReflectionType).Should().Be(true);
			(null as ReflectionType != secondWithNull).Should().Be(true);
			(firstWithNull != secondWithNull).Should().Be(false);

			(firstWithNull != second).Should().Be(true);
			(first != secondWithNull).Should().Be(true);
		}
	}
}