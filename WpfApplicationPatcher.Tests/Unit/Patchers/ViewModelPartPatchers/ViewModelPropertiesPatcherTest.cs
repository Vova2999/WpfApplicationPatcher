using System;
using System.Windows.Input;
using FluentAssertions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Helpers;
using WpfApplicationPatcher.Patchers.ViewModelPartPatchers;
using WpfApplicationPatcher.Tests.Fake;
using WpfApplicationPatcher.Tests.Fake.Types;
using WpfApplicationPatcher.Types.Attributes.Properties;
using WpfApplicationPatcher.Types.Attributes.ViewModels;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Tests.Unit.Patchers.ViewModelPartPatchers {
	[TestFixture]
	public class ViewModelPropertiesPatcherTest : ViewModelPartPatcherBase {
		private ViewModelPropertiesPatcher viewModelPropertiesPatcher;

		[SetUp]
		public void SetUp() {
			viewModelPropertiesPatcher = new ViewModelPropertiesPatcher(MonoCecilFactory.Object);
		}

		[Test, DoNotAddLogOffset]
		public void ValidProperty() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(int)) } } },
					PropertyType = new FakeType(typeof(int))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().NotThrow();
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithoutGetMethodAccessor() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(int)) } } },
					PropertyType = new FakeType(typeof(int))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().Throw<Exception>().WithMessage("Internal error of property patching").And.InnerException?.Message.Should().Be("Patching property must have get method accessor");
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithoutSetMethodAccessor() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					GetMethod = new FakeMethod(),
					PropertyType = new FakeType(typeof(int))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().Throw<Exception>().WithMessage("Internal error of property patching").And.InnerException?.Message.Should().Be("Patching property must have set method accessor");
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithCommandType() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(ICommand)) } } },
					PropertyType = new FakeType(typeof(ICommand))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().NotThrow();
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithCommandTypeAndPatchingPropertyAttribute() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					Attributes = new[] { new FakeAttribute(new PatchingPropertyAttribute()) },
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(ICommand)) } } },
					PropertyType = new FakeType(typeof(ICommand))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().Throw<Exception>().WithMessage("Internal error of property patching").And.InnerException?.Message.Should().Be("Patching property type cannot be inherited from ICommand");
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithRelayCommandType() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(RelayCommand)) } } },
					PropertyType = new FakeType(typeof(RelayCommand))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().NotThrow();
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithRelayCommandTypeAndPatchingPropertyAttribute() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					Attributes = new[] { new FakeAttribute(new PatchingPropertyAttribute()) },
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(RelayCommand)) } } },
					PropertyType = new FakeType(typeof(RelayCommand))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().Throw<Exception>().WithMessage("Internal error of property patching").And.InnerException?.Message.Should().Be("Patching property type cannot be inherited from ICommand");
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithCommandTypeAndViewModelHaveSelectivelyPatchingType() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddAttribute(new FakeAttribute(new PatchingViewModelAttribute(ViewModelPatchingType.Selectively)))
				.AddProperty(new FakeProperty {
					Name = "Property",
					Attributes = new[] { new FakeAttribute(new PatchingPropertyAttribute()) },
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(ICommand)) } } },
					PropertyType = new FakeType(typeof(ICommand))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().Throw<Exception>().WithMessage("Internal error of property patching").And.InnerException?.Message.Should().Be("Patching property type cannot be inherited from ICommand");
		}

		[Test, DoNotAddLogOffset]
		public void PropertyWithNameInLowerCase() {
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "property",
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(int)) } } },
					PropertyType = new FakeType(typeof(int))
				})
				.Build();

			var action = new Action(() => viewModelPropertiesPatcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All));
			action.Should().Throw<Exception>().WithMessage("Internal error of property patching").And.InnerException?.Message.Should().Be("First character of property name must be to upper case");
		}
	}
}