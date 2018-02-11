using System.Linq;
using GalaSoft.MvvmLight;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Moq;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Factories;
using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;
using WpfApplicationPatcher.Patchers.ViewModelPartPatchers;
using WpfApplicationPatcher.Tests.Fake;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Tests.Unit {
	[TestFixture]
	public class WpfApplicationPatcherProcessorTest {
		[Test]
		public void ExecutePatсhersTest() {
			const string assemblyPath = "AssemblyPath";
			const int patchersCount = 5;

			var reflectionAssemblyFactory = new Mock<ReflectionAssemblyFactory>(MockBehavior.Strict);
			var reflectionAssembly = new Mock<ReflectionAssembly>(MockBehavior.Strict, null);
			reflectionAssemblyFactory.Setup(factory => factory.Create(assemblyPath)).Returns(() => reflectionAssembly.Object);

			var monoCecilAssemblyFactory = new Mock<MonoCecilAssemblyFactory>(MockBehavior.Strict);
			var monoCecilAssembly = new Mock<MonoCecilAssembly>(MockBehavior.Strict, null);
			monoCecilAssemblyFactory.Setup(factory => factory.Create(assemblyPath)).Returns(() => monoCecilAssembly.Object);
			monoCecilAssemblyFactory.Setup(factory => factory.Save(monoCecilAssembly.Object, assemblyPath));

			var commonAssemblyContainerFactory = new Mock<CommonAssemblyContainerFactory>(MockBehavior.Strict);
			var commonAssemblyContainer = new Mock<CommonAssemblyContainer>(MockBehavior.Strict, null);
			commonAssemblyContainerFactory
				.Setup(factory => factory.Create(reflectionAssembly.Object, monoCecilAssembly.Object))
				.Returns(() => commonAssemblyContainer.Object);

			var patchers = Enumerable.Range(0, patchersCount)
				.Select(x => {
					var patcher = new Mock<IPatcher>();
					patcher.Setup(p => p.Patch(monoCecilAssembly.Object, commonAssemblyContainer.Object));
					return patcher;
				})
				.ToArray();


			var wpfApplicationPatcherProcessor = new WpfApplicationPatcherProcessor(
				reflectionAssemblyFactory.Object,
				monoCecilAssemblyFactory.Object,
				commonAssemblyContainerFactory.Object,
				patchers.Select(p => p.Object).ToArray());

			wpfApplicationPatcherProcessor.PatchApplication(assemblyPath);


			reflectionAssemblyFactory.Verify(factory => factory.Create(It.IsAny<string>()), Times.Once);
			reflectionAssemblyFactory.Verify(factory => factory.Create(assemblyPath), Times.Once);

			monoCecilAssemblyFactory.Verify(factory => factory.Create(It.IsAny<string>()), Times.Once);
			monoCecilAssemblyFactory.Verify(factory => factory.Create(assemblyPath), Times.Once);

			commonAssemblyContainerFactory.Verify(factory => factory.Create(It.IsAny<ReflectionAssembly>(), It.IsAny<MonoCecilAssembly>()), Times.Once);
			commonAssemblyContainerFactory.Verify(factory => factory.Create(reflectionAssembly.Object, monoCecilAssembly.Object), Times.Once);

			foreach (var patcher in patchers) {
				patcher.Verify(p => p.Patch(It.IsAny<MonoCecilAssembly>(), It.IsAny<CommonAssemblyContainer>()), Times.Once);
				patcher.Verify(p => p.Patch(monoCecilAssembly.Object, commonAssemblyContainer.Object), Times.Once);
			}

			monoCecilAssemblyFactory.Verify(factory => factory.Save(It.IsAny<MonoCecilAssembly>(), It.IsAny<string>()), Times.Once);
			monoCecilAssemblyFactory.Verify(factory => factory.Save(monoCecilAssembly.Object, assemblyPath), Times.Once);
		}

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

			var viewModelPartPropertiesPatcher = new ViewModelPartPropertiesPatcher(monoCecilFactory.Object);

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