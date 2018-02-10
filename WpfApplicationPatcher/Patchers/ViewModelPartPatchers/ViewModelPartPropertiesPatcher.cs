using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Mono.Cecil;
using Mono.Cecil.Cil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Factories;
using WpfApplicationPatcher.Core.Helpers;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Types.Attributes.Properties;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers.ViewModelPartPatchers {
	public class ViewModelPartPropertiesPatcher : IViewModelPartPatcher {
		private const string propertyHaveCommandTypeErrorMessage = "Patching property type cannot be inherited from ICommand";
		private const string propertyNameStartsWithInLowerCaseErrorMessage = "First character of property name must be to upper case";
		private const string propertyGetMethodMissing = "Patching property must have get method accessor";
		private const string propertySetMethodMissing = "Patching property must have set method accessor";

		private readonly MonoCecilFactory monoCecilFactory;
		private readonly Log log;

		public ViewModelPartPropertiesPatcher(MonoCecilFactory monoCecilFactory) {
			this.monoCecilFactory = monoCecilFactory;
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBase, CommonType viewModel, ViewModelPatchingType viewModelPatchingType) {
			log.Info($"Patching {viewModel.FullName} properties...");

			var properties = GetViewModelCommonProperties(viewModel, viewModelPatchingType);
			if (!properties.Any()) {
				log.Info("Not found properties");
				return;
			}

			log.Debug("Properties found:", properties.Select(property => property.FullName));

			foreach (var property in properties) {
				log.Info($"Patching {property.FullName}...");
				PatchProprty(monoCecilAssembly, viewModelBase, viewModel, property);
				log.Info($"{property.FullName} was patched");
			}

			log.Info($"Properties {viewModel.FullName} was patched");
		}

		[DoNotAddLogOffset]
		private CommonProperty[] GetViewModelCommonProperties(CommonType viewModel, ViewModelPatchingType viewModelPatchingType) {
			switch (viewModelPatchingType) {
				case ViewModelPatchingType.All:
					return viewModel.Properties
						.Where(property =>
							property.Attributes.NotContains(typeof(NotPatchingPropertyAttribute)) &&
							(property.Attributes.Contains(typeof(PatchingPropertyAttribute)) || property.IsNot(typeof(ICommand))))
						.ToArray();
				case ViewModelPatchingType.Selectively:
					return viewModel.Properties
						.Where(property => property.Attributes.Contains(typeof(PatchingPropertyAttribute)))
						.ToArray();
				default:
					log.Error($"Not implement patching for properties with {nameof(ViewModelPatchingType)} = {viewModelPatchingType}");
					throw new ArgumentOutOfRangeException(nameof(viewModelPatchingType), viewModelPatchingType, null);
			}
		}

		private void PatchProprty(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBase, CommonType viewModel, CommonProperty property) {
			CheckProperty(property);

			var propertyName = property.Name;
			log.Debug($"Property name: {propertyName}");

			var backgroundFieldName = $"{char.ToLower(propertyName.First())}{propertyName.Substring(1)}";
			log.Debug($"Background field name: {backgroundFieldName}");

			var backgroundField = viewModel.Fields.FirstOrDefault(field => field.Name == backgroundFieldName)?.MonoCecilField;

			if (backgroundField == null) {
				backgroundField = monoCecilFactory.CreateField(backgroundFieldName, FieldAttributes.Private, property.MonoCecilProperty.PropertyType);
				viewModel.MonoCecilType.AddField(backgroundField);
				log.Debug("Background field was created");
			}
			else
				log.Debug("Background field was connected");

			GenerateGetMethodBody(property.MonoCecilProperty, backgroundField);
			GenerateSetMethodBody(monoCecilAssembly, viewModelBase, property.MonoCecilProperty, propertyName, backgroundField);
		}

		[DoNotAddLogOffset]
		private void CheckProperty(CommonProperty property) {
			if (property.Is(typeof(ICommand))) {
				log.Error(propertyHaveCommandTypeErrorMessage);
				throw new Exception("Internal error of property patching", new Exception(propertyHaveCommandTypeErrorMessage));
			}

			if (char.IsUpper(property.MonoCecilProperty.Name.First()))
				return;

			log.Error(propertyNameStartsWithInLowerCaseErrorMessage);
			throw new Exception("Internal error of property patching", new Exception(propertyNameStartsWithInLowerCaseErrorMessage));
		}

		[DoNotAddLogOffset]
		private void GenerateGetMethodBody(MonoCecilProperty property, MonoCecilField backgroundField) {
			log.Info("Generate get method body...");
			var propertyGetMethod = property.GetMethod;
			if (propertyGetMethod == null) {
				log.Error(propertyGetMethodMissing);
				throw new Exception("Internal error of property patching", new Exception(propertyGetMethodMissing));
			}

			var getMethodBodyInstructions = propertyGetMethod.Body.Instructions;
			getMethodBodyInstructions.Clear();
			getMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldarg_0));
			getMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldfld, backgroundField));
			getMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ret));
			propertyGetMethod.RemoveAttribute(typeof(CompilerGeneratedAttribute).FullName);
			log.Info("Get method body was generated");
		}

		[DoNotAddLogOffset]
		private void GenerateSetMethodBody(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBase, MonoCecilProperty property, string propertyName, MonoCecilField backgroundField) {
			log.Info("Generate method reference on Set method in ViewModelBase...");
			var propertySetMethod = property.SetMethod;
			if (propertySetMethod == null) {
				log.Error(propertyGetMethodMissing);
				throw new Exception("Internal error of property patching", new Exception(propertyGetMethodMissing));
			}

			var setMethodFromViewModelBase = monoCecilFactory.CreateGenericInstanceMethod(GetSetMethodFromViewModelBase(viewModelBase.MonoCecilType));
			setMethodFromViewModelBase.AddGenericArgument(property.PropertyType);
			var setMethodInViewModelBaseWithGenericParameter = monoCecilAssembly.MainModule.Import(setMethodFromViewModelBase);

			log.Info("Generate set method body...");
			var setMethodBodyInstructions = propertySetMethod.Body.Instructions;
			setMethodBodyInstructions.Clear();
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldarg_0));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldstr, propertyName));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldarg_0));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldflda, backgroundField));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldarg_1));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ldc_I4_0));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Call, setMethodInViewModelBaseWithGenericParameter));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Pop));
			setMethodBodyInstructions.Add(monoCecilFactory.CreateInstruction(OpCodes.Ret));
			propertySetMethod.RemoveAttribute(typeof(CompilerGeneratedAttribute).FullName);
			log.Info("Set method body was generated");
		}

		private static MonoCecilMethod setMethodInViewModelBase;
		private static MonoCecilMethod GetSetMethodFromViewModelBase(MonoCecilType viewModelBaseType) {
			return setMethodInViewModelBase ?? (setMethodInViewModelBase =
				viewModelBaseType.Methods.Single(method =>
					method.Name == "Set" &&
					method.Parameters.Count() == 4 &&
					method.GetParameterByIndex(0).ParameterType.FullName == typeof(string).FullName &&
					method.GetParameterByIndex(1).ParameterType.FullName == "T&" &&
					method.GetParameterByIndex(2).ParameterType.FullName == "T" &&
					method.GetParameterByIndex(3).ParameterType.FullName == typeof(bool).FullName));
		}
	}
}