using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Castle.Core.Internal;
using GalaSoft.MvvmLight.CommandWpf;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Helpers;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Exceptions;
using WpfApplicationPatcher.Types.Attributes.Commands.Methods;
using WpfApplicationPatcher.Types.Attributes.Commands.Properties;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers.ViewModel.Parts {
	public class ViewModelCommandsPatcher : IViewModelPartPatcher {
		private const string commandPropertyEndsWith = "Command";
		private const string commandExecuteMethodEndsWith = "Method";
		private const string commandCanExecuteMethodStartsWith = "Can";

		private readonly Log log;

		public ViewModelCommandsPatcher() {
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBase, CommonType viewModel, ViewModelPatchingType viewModelPatchingType) {
			log.Info($"Patching {viewModel.FullName} commands...");

			var commandMethods = GetCommonCommandMethods(viewModel, viewModelPatchingType);
			var commandProperties = GetCommonCommandProperties(viewModel, viewModelPatchingType);

			var commandsMembers = JoinMembers(commandMethods, commandProperties);
		}

		[DoNotAddLogOffset]
		private CommonMethod[] GetCommonCommandMethods(CommonType viewModel, ViewModelPatchingType viewModelPatchingType) {
			switch (viewModelPatchingType) {
				case ViewModelPatchingType.All:
					return viewModel.Methods
						.Where(method =>
							method.Attributes.NotContains(typeof(NotPatchingToCommandAttribute)) &&
							(method.Attributes.Contains(typeof(PatchingToCommandAttribute)) || method.IsPublic))
						.ToArray();
				case ViewModelPatchingType.Selectively:
					return viewModel.Methods
						.Where(method => method.Attributes.NotContains(typeof(NotPatchingToCommandAttribute)))
						.ToArray();
				default:
					log.Error($"Not implement patching for commands with {nameof(ViewModelPatchingType)} = {viewModelPatchingType}");
					throw new ArgumentOutOfRangeException(nameof(viewModelPatchingType), viewModelPatchingType, null);
			}
		}

		[DoNotAddLogOffset]
		private CommonProperty[] GetCommonCommandProperties(CommonType viewModel, ViewModelPatchingType viewModelPatchingType) {
			switch (viewModelPatchingType) {
				case ViewModelPatchingType.All:
					return viewModel.Properties
						.Where(property =>
							property.Attributes.NotContains(typeof(NotCombineWithMethodAttribute)) &&
							(property.Attributes.Contains(typeof(CombineWithMethodAttribute)) || property.IsInheritedFrom(typeof(ICommand))))
						.ToArray();
				case ViewModelPatchingType.Selectively:
					return viewModel.Properties
						.Where(property => property.Attributes.Contains(typeof(CombineWithMethodAttribute)))
						.ToArray();
				default:
					log.Error($"Not implement patching for commands with {nameof(ViewModelPatchingType)} = {viewModelPatchingType}");
					throw new ArgumentOutOfRangeException(nameof(viewModelPatchingType), viewModelPatchingType, null);
			}
		}

		[DoNotAddLogOffset]
		private CommandMembers[] JoinMembers(CommonMethod[] commandMethods, CommonProperty[] commandProperties) {
			var commandsMembers = new List<CommandMembers>();

			foreach (var commandProperty in commandProperties) {
				CheckCommandProperty(commandProperty);

				var combineWithMethodAttribute = commandProperty.GetReflectionAttribute<CombineWithMethodAttribute>();

				var commandPropertyName = commandProperty.Name;
				var executeMethodName = combineWithMethodAttribute?.ExecuteMethodName ?? commandPropertyName.Substring(0, commandPropertyName.Length - commandCanExecuteMethodStartsWith.Length);
				var canExecuteMethodName = combineWithMethodAttribute?.CanExecuteMethodName ?? $"Can{executeMethodName}";

				commandsMembers.Add(new CommandMembers());
			}
		}

		[DoNotAddLogOffset]
		private void CheckCommandProperty(CommonProperty commandProperty) {
			if (commandProperty.IsNot(typeof(ICommand)) || commandProperty.IsNot(typeof(RelayCommand))) {
				const string errorMessage = "Patching command property type must be ICommand or RelayCommand";

				log.Error(errorMessage);
				throw new PropertyPatchingException(errorMessage, commandProperty.FullName);
			}

			if (char.IsLower(commandProperty.Name.First())) {
				const string errorMessage = "First character of property name must be to upper case";

				log.Error(errorMessage);
				throw new PropertyPatchingException(errorMessage, commandProperty.FullName);
			}

			// ReSharper disable once InvertIf
			if (!commandProperty.Name.EndsWith(commandPropertyEndsWith)) {
				var errorMessage = $"Patching command property name must be ends with '{commandPropertyEndsWith}'";

				log.Error(errorMessage);
				throw new PropertyPatchingException(errorMessage, commandProperty.FullName);
			}
		}

		private class CommandMembers {
			public CommonMethod ExecuteMethod { get; set; }
			public CommonMethod CanExecuteMethod { get; set; }
			public CommonProperty CommandProperty { get; set; }
		}
	}
}