using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using WpfApplicationPatcher.Types.Attributes.Classes;
using WpfApplicationPatcher.Types.Attributes.Properties;

namespace WpfApplicationPatcher.Helpers {
	// ReSharper disable InconsistentNaming

	public static class TypeNames {
		public static readonly string ICommand = typeof(ICommand).FullName;
		public static readonly string RelayCommand = typeof(RelayCommand).FullName;
		public static readonly string NotPatchingPropertyAttribute = typeof(NotPatchingPropertyAttribute).FullName;
		public static readonly string PatchingPropertyAttribute = typeof(PatchingPropertyAttribute).FullName;
		public static readonly string PatchingViewModelAttribute = typeof(PatchingViewModelAttribute).FullName;
		public static readonly string ViewModelBase = typeof(ViewModelBase).FullName;
	}
}