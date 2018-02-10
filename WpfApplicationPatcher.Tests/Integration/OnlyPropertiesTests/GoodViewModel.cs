using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GalaSoft.MvvmLight;
using NUnit.Framework;

namespace WpfApplicationPatcher.Tests.Integration.OnlyPropertiesTests {
	public class GoodViewModel : ViewModelBase {
		public int Number { get; set; }
		public string Line { get; set; }
		public double Value { get; set; }
		public GoodViewModelAdditionalType AdditionalType { get; set; }
	}

	public class GoodViewModelAdditionalType {
	}

	[TestFixture]
	public class GoodViewModelTest {
		private GoodViewModel goodViewModel;
		private List<string> changedProperties;

		[OneTimeSetUp]
		public void OneTimeSetUp() {
			goodViewModel = new GoodViewModel();
			goodViewModel.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

			changedProperties = new List<string>();
		}

		[SetUp]
		public void SetUp() {
			changedProperties.Clear();
		}

		[Test, TestCaseSource(nameof(PropertyTestSource))]
		public void PropertiesTest(Action<GoodViewModel> setProperty, string propertyName) {
			for (var i = 1; i <= 10; i++) {
				setProperty(goodViewModel);
				changedProperties.Should().BeEquivalentTo(Enumerable.Range(0, i).Select(_ => propertyName));
			}
		}

		private static IEnumerable<TestCaseData> PropertyTestSource() {
			var random = new Random(1);
			yield return PropertyTestCaseData(goodViewModel => goodViewModel.Number = random.Next(), nameof(GoodViewModel.Number));
			yield return PropertyTestCaseData(goodViewModel => goodViewModel.Line = random.Next().ToString(), nameof(GoodViewModel.Line));
			yield return PropertyTestCaseData(goodViewModel => goodViewModel.Value = (double)random.Next() / 100, nameof(GoodViewModel.Value));
			yield return PropertyTestCaseData(goodViewModel => goodViewModel.AdditionalType = new GoodViewModelAdditionalType(), nameof(GoodViewModel.AdditionalType));
		}

		private static TestCaseData PropertyTestCaseData(Action<GoodViewModel> setProperty, string propertyName) {
			return new TestCaseData(setProperty, propertyName).SetName($"{propertyName}Test");
		}
	}
}