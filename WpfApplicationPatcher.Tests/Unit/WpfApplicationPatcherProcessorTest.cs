using NUnit.Framework;

namespace WpfApplicationPatcher.Tests.Unit {
	[TestFixture]
	public class WpfApplicationPatcherProcessorTest {
		//[Test]
		//public void ExecutePatсhers() {
		//	const string assemblyPath = "AssemblyPath";

		//	var reflectionAssemblyFactory = new Mock<ReflectionAssemblyFactory>(MockBehavior.Strict);
		//	var reflectionAssembly = new Mock<ReflectionAssembly>(MockBehavior.Strict);
		//	reflectionAssemblyFactory.Setup(factory => factory.Craete(assemblyPath)).Returns(() => reflectionAssembly.Object);

		//	var assemblyDefinitionFactory = new Mock<AssemblyDefinitionFactory>(MockBehavior.Strict);
		//	var assemblyDefinition = new Mock<AssemblyDefinition>(MockBehavior.Strict);
		//	assemblyDefinitionFactory.Setup(factory => factory.Create(assemblyPath)).Returns(() => assemblyDefinition.Object);

		//	var assemblyContainerFactory = new Mock<AssemblyContainerFactory>(MockBehavior.Strict);
		//	var assemblyContainer = new Mock<CommonAssemblyContainer>(MockBehavior.Strict);
		//	assemblyContainerFactory.Setup(factory => factory.Create(reflectionAssembly.Object, assemblyDefinition.Object)).Returns(() => assemblyContainer.Object);

		//	var patchers = new Mock<IPatcher[]>(MockBehavior.Strict);

		//	var wpfApplicationPatcherProcessor = new WpfApplicationPatcherProcessor(
		//		reflectionAssemblyFactory.Object,
		//		assemblyDefinitionFactory.Object,
		//		assemblyContainerFactory.Object,
		//		patchers.Object);

		//	wpfApplicationPatcherProcessor.PatchApplication(assemblyPath);
		//}
	}
}