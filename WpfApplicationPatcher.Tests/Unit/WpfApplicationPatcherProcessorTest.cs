using System.Linq;
using GalaSoft.MvvmLight;
using Moq;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Factories;
using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Core.Types.Reflection;
using WpfApplicationPatcher.Tests.Fake;

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
			var commonAssemblyContainer = new CommonTypeContainer(new[] { FakeCommonTypeBuilder.Create(typeof(ViewModelBase)).Build() });
			commonAssemblyContainerFactory
				.Setup(factory => factory.Create(reflectionAssembly.Object, monoCecilAssembly.Object))
				.Returns(() => commonAssemblyContainer);

			var patchers = Enumerable.Range(0, patchersCount)
				.Select(x => {
					var patcher = new Mock<IPatcher>();
					patcher.Setup(p => p.Patch(monoCecilAssembly.Object, commonAssemblyContainer));
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
				patcher.Verify(p => p.Patch(It.IsAny<MonoCecilAssembly>(), It.IsAny<CommonTypeContainer>()), Times.Once);
				patcher.Verify(p => p.Patch(monoCecilAssembly.Object, commonAssemblyContainer), Times.Once);
			}

			monoCecilAssemblyFactory.Verify(factory => factory.Save(It.IsAny<MonoCecilAssembly>(), It.IsAny<string>()), Times.Once);
			monoCecilAssemblyFactory.Verify(factory => factory.Save(monoCecilAssembly.Object, assemblyPath), Times.Once);
		}
	}
}