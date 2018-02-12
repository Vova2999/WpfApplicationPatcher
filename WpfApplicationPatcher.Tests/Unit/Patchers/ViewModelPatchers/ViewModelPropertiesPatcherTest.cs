using GalaSoft.MvvmLight;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Moq;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Factories;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Patchers.ViewModelPartPatchers;
using WpfApplicationPatcher.Tests.Fake;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Tests.Unit.Patchers.ViewModelPatchers {
	[TestFixture]
	public class ViewModelPropertiesPatcherTest {
		[Test]
		public void A() {
			var monoCecilAssembly = new Mock<MonoCecilAssembly>(null);
			monoCecilAssembly.Setup(assembly => assembly.MainModule).Returns(() => new Mock<MonoCecilModule>(null).Object);

			var monoCecilFactory = new Mock<MonoCecilFactory>();
			monoCecilFactory
				.Setup(factory => factory.CreateField(It.IsAny<string>(), It.IsAny<FieldAttributes>(), It.IsAny<MonoCecilTypeReference>()))
				.Returns(() => new Mock<MonoCecilField>(null).Object);
			monoCecilFactory
				.Setup(factory => factory.CreateGenericInstanceMethod(It.IsAny<MonoCecilMethod>()))
				.Returns(() => new Mock<MonoCecilGenericInstanceMethod>(null).Object);
			monoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);
			monoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>(), It.IsAny<string>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);
			monoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>(), It.IsAny<MonoCecilField>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);
			monoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>(), It.IsAny<MonoCecilMethodReference>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);

			var viewModelPartPropertiesPatcher = new ViewModelPropertiesPatcher(monoCecilFactory.Object);

			var viewModelBase = new FakeCommonTypeBuilder("ViewModelBase")
				.AddMethod(new FakeMethod {
						Name = "Set",
						Parameters = new[] {
							new FakeParameter { ParameterType = new FakeType(typeof(string)) },
							new FakeParameter { ParameterType = new FakeType("T&") },
							new FakeParameter { ParameterType = new FakeType("T") },
							new FakeParameter { ParameterType = new FakeType(typeof(bool)) }
						}
					}
				)
				.Build();

			var viewModel = new FakeCommonTypeBuilder("ViewModel", typeof(ViewModelBase))
				.AddProperty(new FakeProperty {
					Name = "Property",
					GetMethod = new FakeMethod(),
					SetMethod = new FakeMethod { Parameters = new[] { new FakeParameter { ParameterType = new FakeType(typeof(int)) } } },
					PropertyType = new FakeType(typeof(int))
				})
				.Build();

			viewModelPartPropertiesPatcher.Patch(monoCecilAssembly.Object, viewModelBase, viewModel, ViewModelPatchingType.All);
		}
	}
}