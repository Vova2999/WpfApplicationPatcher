using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Types.Reflection;

namespace WpfApplicationPatcher.Tests.Unit {
	[TestFixture]
	public class ObjectBaseCompareTest {
		private const string nullName = "null";
		private const string notNullName = "not null";
		private const string withNullName = "null instance";

		private static readonly TestReflectionType firstNull = new TestReflectionType(nullName, null);
		private static readonly TestReflectionType secondNull = new TestReflectionType(nullName, null);
		private static readonly TestReflectionType firstNotNull = new TestReflectionType(notNullName, CreateReflectionType(typeof(ObjectBaseCompareTest)));
		private static readonly TestReflectionType secondNotNull = new TestReflectionType(notNullName, CreateReflectionType(typeof(ObjectBaseCompareTest)));
		private static readonly TestReflectionType firstWithNull = new TestReflectionType(withNullName, CreateReflectionType(null));
		private static readonly TestReflectionType secondWithNull = new TestReflectionType(withNullName, CreateReflectionType(null));

		private static ReflectionType CreateReflectionType(Type type) {
			return new Mock<ReflectionType>(type) { CallBase = true }.Object;
		}

		[Test, TestCaseSource(nameof(CompareTestCaseSource), new object[] { true })]
		public void EqualityCompareTest(ReflectionType left, ReflectionType right, bool expectedResult) {
			(left == right).Should().Be(expectedResult);
		}

		[Test, TestCaseSource(nameof(CompareTestCaseSource), new object[] { false })]
		public void InequalityCompareTest(ReflectionType left, ReflectionType right, bool expectedResult) {
			(left != right).Should().Be(expectedResult);
		}

		private static IEnumerable<TestCaseData> CompareTestCaseSource(bool equalityMode) {
			yield return CreateTestCaseData(firstNull, secondNull, equalityMode);
			yield return CreateTestCaseData(firstNotNull, secondNull, !equalityMode);
			yield return CreateTestCaseData(firstNull, secondNotNull, !equalityMode);
			yield return CreateTestCaseData(firstNotNull, secondNotNull, equalityMode);

			yield return CreateTestCaseData(firstWithNull, secondNull, !equalityMode);
			yield return CreateTestCaseData(firstNull, secondWithNull, !equalityMode);
			yield return CreateTestCaseData(firstWithNull, secondWithNull, equalityMode);

			yield return CreateTestCaseData(firstWithNull, secondNotNull, !equalityMode);
			yield return CreateTestCaseData(firstNotNull, secondWithNull, !equalityMode);
		}

		private static TestCaseData CreateTestCaseData(TestReflectionType left, TestReflectionType right, bool expectedResult) {
			return new TestCaseData(left.ReflectionType, right.ReflectionType, expectedResult)
				.SetName($"left: {left.Name}, right: {right.Name}, expectedResult: {expectedResult}");
		}

		private class TestReflectionType {
			public readonly string Name;
			public readonly ReflectionType ReflectionType;

			public TestReflectionType(string name, ReflectionType reflectionType) {
				Name = name;
				ReflectionType = reflectionType;
			}
		}
	}
}