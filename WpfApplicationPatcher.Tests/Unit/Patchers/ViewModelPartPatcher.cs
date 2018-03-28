using System.Linq;
using GalaSoft.MvvmLight;
using Moq;
using NUnit.Framework;
using WpfApplicationPatcher.Core.Types;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Patchers.ViewModel;
using WpfApplicationPatcher.Tests.Fake;
using WpfApplicationPatcher.Tests.Fake.Types;
using WpfApplicationPatcher.Types.Attributes.ViewModels;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Tests.Unit.Patchers {
	[TestFixture]
	public class ViewModelPartPatcher : ViewModelPartPatcherBase {
		[Test]
		public void AllEmpty() {
			var viewModelPatcher = new ViewModelPatcher(Enumerable.Empty<IViewModelPartPatcher>().ToArray());
			viewModelPatcher.Patch(MonoCecilAssembly.Object, new CommonTypeContainer(Enumerable.Empty<CommonType>().ToArray()));
		}

		[Test]
		public void ViewModelPartPatchersEmpty() {
			var viewModelPatcher = new ViewModelPatcher(Enumerable.Empty<IViewModelPartPatcher>().ToArray());
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase)).WhereFrom(MonoCecilModule.Object).Build();

			viewModelPatcher.Patch(MonoCecilAssembly.Object, new CommonTypeContainer(new[] { ViewModelBase, viewModel }));
		}

		[Test]
		public void OnePartPatcher() {
			var viewModelPartPatcher = new Mock<IViewModelPartPatcher>();
			var viewModelPatcher = new ViewModelPatcher(new[] { viewModelPartPatcher.Object });
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase)).WhereFrom(MonoCecilModule.Object).Build();

			viewModelPatcher.Patch(MonoCecilAssembly.Object, new CommonTypeContainer(new[] { ViewModelBase, viewModel }));

			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(It.IsAny<MonoCecilAssembly>(), It.IsAny<CommonType>(), It.IsAny<CommonType>(), It.IsAny<ViewModelPatchingType>()), Times.Once);
			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All), Times.Once);
		}

		[Test]
		public void ManyPartPatchersAndViewModels() {
			const int viewModelCount = 5;
			const int partPatcherCount = 4;

			var viewModelPartPatchers = Enumerable.Range(0, partPatcherCount).Select(x => new Mock<IViewModelPartPatcher>()).ToArray();
			var viewModelPatcher = new ViewModelPatcher(viewModelPartPatchers.Select(patcher => patcher.Object).ToArray());
			var viewModels = Enumerable.Range(0, viewModelCount)
				.Select(x => FakeCommonTypeBuilder.Create($"ViewModel{x + 1}", typeof(ViewModelBase)).WhereFrom(MonoCecilModule.Object).Build())
				.ToArray();

			viewModelPatcher.Patch(MonoCecilAssembly.Object, new CommonTypeContainer(new[] { ViewModelBase }.Concat(viewModels).ToArray()));

			foreach (var viewModelPartPatcher in viewModelPartPatchers) {
				viewModelPartPatcher
					.Verify(patcher => patcher.Patch(It.IsAny<MonoCecilAssembly>(), It.IsAny<CommonType>(), It.IsAny<CommonType>(), It.IsAny<ViewModelPatchingType>()), Times.Exactly(viewModelCount));
				foreach (var viewModel in viewModels)
					viewModelPartPatcher
						.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.All), Times.Once);
			}
		}

		[Test]
		public void ViewModelsFromNotCurrentModule() {
			var viewModelPartPatcher = new Mock<IViewModelPartPatcher>();
			var viewModelPatcher = new ViewModelPatcher(new[] { viewModelPartPatcher.Object });
			var viewModel1 = FakeCommonTypeBuilder.Create("ViewModel1", typeof(ViewModelBase)).WhereFrom(MonoCecilModule.Object).Build();
			var viewModel2 = FakeCommonTypeBuilder.Create("ViewModel2", typeof(ViewModelBase)).Build();
			var viewModel3 = FakeCommonTypeBuilder.Create("ViewModel3", typeof(ViewModelBase)).WhereFrom(MonoCecilModule.Object).Build();
			var viewModel4 = FakeCommonTypeBuilder.Create("ViewModel4", typeof(ViewModelBase)).Build();

			viewModelPatcher.Patch(MonoCecilAssembly.Object, new CommonTypeContainer(new[] { ViewModelBase, viewModel1, viewModel2, viewModel3, viewModel4 }));

			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(It.IsAny<MonoCecilAssembly>(), It.IsAny<CommonType>(), It.IsAny<CommonType>(), It.IsAny<ViewModelPatchingType>()), Times.Exactly(2));
			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel1, ViewModelPatchingType.All), Times.Once);
			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel3, ViewModelPatchingType.All), Times.Once);
		}

		[Test]
		public void ViewModelWithPatchingType() {
			var viewModelPartPatcher = new Mock<IViewModelPartPatcher>();
			var viewModelPatcher = new ViewModelPatcher(new[] { viewModelPartPatcher.Object });
			var viewModel = FakeCommonTypeBuilder.Create("ViewModel", typeof(ViewModelBase))
				.AddAttribute(new FakeAttribute(new PatchingViewModelAttribute(ViewModelPatchingType.Selectively)))
				.WhereFrom(MonoCecilModule.Object)
				.Build();

			viewModelPatcher.Patch(MonoCecilAssembly.Object, new CommonTypeContainer(new[] { ViewModelBase, viewModel }));

			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(It.IsAny<MonoCecilAssembly>(), It.IsAny<CommonType>(), It.IsAny<CommonType>(), It.IsAny<ViewModelPatchingType>()), Times.Once);
			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel, ViewModelPatchingType.Selectively), Times.Once);
		}

		[Test]
		public void ManyViewModelsWithPatchingTypes() {
			var viewModelPartPatcher = new Mock<IViewModelPartPatcher>();
			var viewModelPatcher = new ViewModelPatcher(new[] { viewModelPartPatcher.Object });
			var viewModel1 = FakeCommonTypeBuilder.Create("ViewModel1", typeof(ViewModelBase))
				.AddAttribute(new FakeAttribute(new PatchingViewModelAttribute(ViewModelPatchingType.Selectively)))
				.WhereFrom(MonoCecilModule.Object)
				.Build();
			var viewModel2 = FakeCommonTypeBuilder.Create("ViewModel2", typeof(ViewModelBase))
				.AddAttribute(new FakeAttribute(new PatchingViewModelAttribute()))
				.WhereFrom(MonoCecilModule.Object)
				.Build();
			var viewModel3 = FakeCommonTypeBuilder.Create("ViewModel2", typeof(ViewModelBase))
				.WhereFrom(MonoCecilModule.Object)
				.Build();
			var viewModel4 = FakeCommonTypeBuilder.Create("ViewModel3", typeof(ViewModelBase))
				.AddAttribute(new FakeAttribute(new PatchingViewModelAttribute(ViewModelPatchingType.Selectively)))
				.Build();

			viewModelPatcher.Patch(MonoCecilAssembly.Object, new CommonTypeContainer(new[] { ViewModelBase, viewModel1, viewModel2, viewModel3, viewModel4 }));

			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(It.IsAny<MonoCecilAssembly>(), It.IsAny<CommonType>(), It.IsAny<CommonType>(), It.IsAny<ViewModelPatchingType>()), Times.Exactly(3));
			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel1, ViewModelPatchingType.Selectively), Times.Once);
			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel2, ViewModelPatchingType.All), Times.Once);
			viewModelPartPatcher
				.Verify(patcher => patcher.Patch(MonoCecilAssembly.Object, ViewModelBase, viewModel3, ViewModelPatchingType.All), Times.Once);
		}
	}
}