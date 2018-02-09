using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;
using WpfApplicationPatcher.Types.Attributes.Properties;
using WpfApplicationPatcher.Types.Common;
using WpfApplicationPatcher.Types.Enums;
using WpfApplicationPatcher.Types.MonoCecil;

namespace WpfApplicationPatcher.Patchers.ViewModelPartPatchers {
	public class ViewModelPartCommandsPatcher : IViewModelPartPatcher {
		private readonly Log log;

		public ViewModelPartCommandsPatcher() {
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBaseAssemblyType, CommonType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType) {
			log.Info($"Patching {viewModelAssemblyType.FullName} commands...");

			var viewModelCommandProperties = GetViewModelCommandProperties(viewModelAssemblyType, viewModelPatchingType);
			//var commandsMembers = new Dictionary<string, CommandMembers>();

			//var properties = viewModelAssemblyType.TypeDefinition.Properties
			//	.Select(property => new { Property = property, PropertyType = property.PropertyType.Resolve() })
			//	.Where(x =>
			//		x.Property.CustomAttributes.NotContains(TypeNames.NotPatchingPropertyAttribute) && (
			//			x.PropertyType.CustomAttributes.Contains(TypeNames.PatchingPropertyAttribute) ||
			//			x.PropertyType.Is(TypeNames.ICommand) || x.PropertyType.Is(TypeNames.RelayCommand)))
			//	.Select(x => x.Property)
			//	.ToArray();

			//foreach (var propertyInfo in viewModelBaseType.Properties.Where(propertyInfo => propertyInfo.PropertyType == commandPropertyType)) {
			//	const string commandPropertyNameEndsWith = "Command";
			//	var name = propertyInfo.Name.EndsWith(commandPropertyNameEndsWith)
			//		? propertyInfo.Name.Substring(0, propertyInfo.Name.Length - commandPropertyNameEndsWith.Length)
			//		: throw new ArgumentException($"Command '{propertyInfo.Name}' name must ends with '{commandPropertyNameEndsWith}'");
			//	if (propertyInfo.GetSetMethod(true) != null)
			//		throw new ArgumentException($"Incorrect signature of command property {propertyInfo.Name}");

			//	if (commandsMembers.TryGetValue(name, out var commandMembers))
			//		commandMembers.CommandPropertyInfo = propertyInfo;
			//	else
			//		commandsMembers.Add(name, new CommandMembers { CommandPropertyInfo = propertyInfo });
			//}
		}

		[DoNotAddLogOffset]
		private CommonProperty[] GetViewModelCommandProperties(CommonType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType) {
			switch (viewModelPatchingType) {
				case ViewModelPatchingType.All:
					return viewModelAssemblyType.Properties
						.Where(assemblyPropertyType =>
							assemblyPropertyType.Attributes.NotContains(typeof(NotPatchingPropertyAttribute)) &&
							(assemblyPropertyType.Attributes.Contains(typeof(PatchingPropertyAttribute)) || assemblyPropertyType.Is(typeof(ICommand))))
						.ToArray();
				case ViewModelPatchingType.Selectively:
					return viewModelAssemblyType.Properties
						.Where(assemblyPropertyType => assemblyPropertyType.Attributes.Contains(typeof(PatchingPropertyAttribute)))
						.ToArray();
				default:
					log.Error($"Not implement patching for properties with {nameof(ViewModelPatchingType)} = {viewModelPatchingType}");
					throw new ArgumentOutOfRangeException(nameof(viewModelPatchingType), viewModelPatchingType, null);
			}
		}

		private class CommandMembers {
			public PropertyInfo CommandPropertyInfo { get; set; }
			public MethodInfo ExecuteMethodInfo { get; set; }
			public MethodInfo CanExecuteMethodInfo { get; set; }
		}
	}
}