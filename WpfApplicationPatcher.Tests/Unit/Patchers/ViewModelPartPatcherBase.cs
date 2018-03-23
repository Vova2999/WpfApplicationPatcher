using GalaSoft.MvvmLight;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Moq;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Factories;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Tests.Fake;
using WpfApplicationPatcher.Tests.Fake.Types;

namespace WpfApplicationPatcher.Tests.Unit.Patchers {
	public abstract class ViewModelPartPatcherBase {
		protected Mock<MonoCecilAssembly> MonoCecilAssembly;
		protected Mock<MonoCecilFactory> MonoCecilFactory;
		protected Mock<MonoCecilModule> MonoCecilModule;
		protected CommonType ViewModelBase;

		[OneTimeSetUp]
		public void InitializeBaseObjects() {
			MonoCecilModule = new Mock<MonoCecilModule>(null);

			MonoCecilAssembly = new Mock<MonoCecilAssembly>(MockBehavior.Strict, null);
			MonoCecilAssembly.Setup(assembly => assembly.MainModule).Returns(() => MonoCecilModule.Object);

			MonoCecilFactory = new Mock<MonoCecilFactory>(MockBehavior.Strict);
			MonoCecilFactory
				.Setup(factory => factory.CreateField(It.IsAny<string>(), It.IsAny<FieldAttributes>(), It.IsAny<MonoCecilTypeReference>()))
				.Returns(() => new Mock<MonoCecilField>(null).Object);
			MonoCecilFactory
				.Setup(factory => factory.CreateGenericInstanceMethod(It.IsAny<MonoCecilMethod>()))
				.Returns(() => new Mock<MonoCecilGenericInstanceMethod>(null).Object);
			MonoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);
			MonoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>(), It.IsAny<string>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);
			MonoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>(), It.IsAny<MonoCecilField>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);
			MonoCecilFactory
				.Setup(factory => factory.CreateInstruction(It.IsAny<OpCode>(), It.IsAny<MonoCecilMethodReference>()))
				.Returns(() => new Mock<MonoCecilInstruction>(null).Object);

			ViewModelBase = FakeCommonTypeBuilder.Create(typeof(ViewModelBase))
				.AddMethod(new FakeMethod {
					Name = "Set",
					Parameters = new[] {
						new FakeParameter { ParameterType = new FakeType(typeof(string)) },
						new FakeParameter { ParameterType = new FakeType("T&") },
						new FakeParameter { ParameterType = new FakeType("T") },
						new FakeParameter { ParameterType = new FakeType(typeof(bool)) }
					}
				})
				.Build();
		}
	}
}