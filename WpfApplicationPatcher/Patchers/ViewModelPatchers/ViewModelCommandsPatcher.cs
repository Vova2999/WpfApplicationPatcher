using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Mono.Cecil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;

namespace WpfApplicationPatcher.Patchers.ViewModelPatchers {
	public class ViewModelCommandsPatcher : IViewModelPatcher {
		private readonly ILog log;

		public ViewModelCommandsPatcher() {
			log = Log.For(this);
		}

		public void Patch(ModuleDefinition module, TypeDefinition viewModelBaseType, TypeDefinition viewModelType) {
			log.Info($"Patching {viewModelType.FullName} commands...");

			var commandsMembers = new Dictionary<string, CommandMembers>();

			var properties = viewModelType.Properties
				.Select(property => new { Property = property, PropertyType = property.PropertyType.Resolve() })
				.Where(x =>
					x.Property.CustomAttributes.NotContains(TypeNames.NotPatchingPropertyAttribute) && (
						x.PropertyType.CustomAttributes.Contains(TypeNames.PatchingPropertyAttribute) ||
						x.PropertyType.Is(TypeNames.ICommand) || x.PropertyType.Is(TypeNames.RelayCommand)))
				.Select(x => x.Property)
				.ToArray();

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

		private class CommandMembers {
			public PropertyInfo CommandPropertyInfo { get; set; }
			public MethodInfo ExecuteMethodInfo { get; set; }
			public MethodInfo CanExecuteMethodInfo { get; set; }
		}
	}
}