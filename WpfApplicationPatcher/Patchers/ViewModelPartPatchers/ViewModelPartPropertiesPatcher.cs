using System;
using System.Linq;
using System.Windows.Input;
using Mono.Cecil;
using Mono.Cecil.Cil;
using WpfApplicationPatcher.Core.Extensions;
using WpfApplicationPatcher.Core.Helpers;
using WpfApplicationPatcher.Core.Types.Common;
using WpfApplicationPatcher.Core.Types.MonoCecil;
using WpfApplicationPatcher.Types.Attributes.Properties;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers.ViewModelPartPatchers {
	public class ViewModelPartPropertiesPatcher : IViewModelPartPatcher {
		private readonly Log log;

		public ViewModelPartPropertiesPatcher() {
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBaseAssemblyType, CommonType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType) {
			log.Info($"Patching {viewModelAssemblyType.FullName} properties...");

			var assemblyPropertyTypes = GetViewModelProperties(viewModelAssemblyType, viewModelPatchingType);
			if (!assemblyPropertyTypes.Any()) {
				log.Info("Not found properties");
				return;
			}

			log.Debug("Properties found:", assemblyPropertyTypes.Select(property => property.FullName));

			foreach (var assemblyPropertyType in assemblyPropertyTypes) {
				log.Info($"Patching {assemblyPropertyType.FullName}...");
				PatchProprty(monoCecilAssembly, viewModelBaseAssemblyType, viewModelAssemblyType, assemblyPropertyType);
				log.Info($"{assemblyPropertyType.FullName} was patched");
			}

			log.Info($"Properties {viewModelAssemblyType.FullName} was patched");
		}

		[DoNotAddLogOffset]
		private CommonProperty[] GetViewModelProperties(CommonType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType) {
			switch (viewModelPatchingType) {
				case ViewModelPatchingType.All:
					return viewModelAssemblyType.Properties
						.Where(assemblyPropertyType =>
							assemblyPropertyType.Attributes.NotContains(typeof(NotPatchingPropertyAttribute)) &&
							(assemblyPropertyType.Attributes.Contains(typeof(PatchingPropertyAttribute)) || assemblyPropertyType.IsNot(typeof(ICommand))))
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

		private void PatchProprty(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBaseAssemblyType, CommonType viewModelAssemblyType, CommonProperty assemblyPropertyType) {
			CheckProperty(assemblyPropertyType);

			var propertyName = assemblyPropertyType.MonoCecilProperty.Name;
			log.Debug($"Property name: {propertyName}");

			var backgroundFieldName = $"{char.ToLower(propertyName.First())}{propertyName.Substring(1)}";
			log.Debug($"Background field name: {backgroundFieldName}");

			var backgroundField = viewModelAssemblyType.MonoCecilType.Fields.FirstOrDefault(field => field.Name == backgroundFieldName);

			if (backgroundField == null) {
				backgroundField = MonoCecilField.Create(backgroundFieldName, FieldAttributes.Private, assemblyPropertyType.MonoCecilProperty.PropertyType);
				viewModelAssemblyType.MonoCecilType.AddField(backgroundField);
				log.Debug("Background field was created");
			}
			else
				log.Debug("Background field was connected");

			GenerateGetMethodBody(assemblyPropertyType.MonoCecilProperty, backgroundField);
			GenerateSetMethodBody(monoCecilAssembly, viewModelBaseAssemblyType, assemblyPropertyType.MonoCecilProperty, propertyName, backgroundField);
		}

		[DoNotAddLogOffset]
		private void CheckProperty(CommonProperty assemblyPropertyType) {
			if (assemblyPropertyType.Is(typeof(ICommand))) {
				log.Error("Patching property type cannot be inherited from ICommand");
				throw new Exception("Internal error of property patching");
			}

			if (char.IsUpper(assemblyPropertyType.MonoCecilProperty.Name.First()))
				return;

			log.Error("First character of property name must be to upper case");
			throw new Exception("Internal error of property patching");
		}

		[DoNotAddLogOffset]
		private void GenerateGetMethodBody(MonoCecilProperty property, MonoCecilField backgroundField) {
			log.Info("Generate get method body...");
			var getMethodBodyInstructions = property.GetMethod.Body.Instructions;
			getMethodBodyInstructions.Clear();
			getMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldarg_0));
			getMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldfld, backgroundField));
			getMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ret));
			log.Info("Get method body was generated");
		}

		[DoNotAddLogOffset]
		private void GenerateSetMethodBody(MonoCecilAssembly monoCecilAssembly, CommonType viewModelBaseAssemblyType, MonoCecilProperty property, string propertyName, MonoCecilField backgroundField) {
			log.Info("Generate method reference on Set method in ViewModelBase...");
			var monoCecilGenericInstanceMethod = MonoCecilGenericInstanceMethod.Create(GetSetMethodInViewModelBase(viewModelBaseAssemblyType.MonoCecilType));
			monoCecilGenericInstanceMethod.AddGenericArgument(property.PropertyType);
			var setMethodInViewModelBaseWithGenericParameter = monoCecilAssembly.MainModule.Import(monoCecilGenericInstanceMethod);

			log.Info("Generate set method body...");
			var setMethodBodyInstructions = property.SetMethod.Body.Instructions;
			setMethodBodyInstructions.Clear();
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldarg_0));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldstr, propertyName));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldarg_0));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldflda, backgroundField));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldarg_1));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ldc_I4_0));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Call, setMethodInViewModelBaseWithGenericParameter));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Pop));
			setMethodBodyInstructions.Add(MonoCecilInstruction.Create(OpCodes.Ret));
			log.Info("Set method body was generated");
		}

		private static MonoCecilMethod setMethodInViewModelBase;
		private static MonoCecilMethod GetSetMethodInViewModelBase(MonoCecilType viewModelBaseType) {
			return setMethodInViewModelBase ?? (setMethodInViewModelBase =
				viewModelBaseType.Methods.Single(method =>
					method.Name == "Set" &&
					method.Parameters.Count() == 4 &&
					method.GetParameterByIndex(0).ParameterType.FullName == typeof(string).FullName &&
					method.GetParameterByIndex(1).ParameterType.IsByReference &&
					method.GetParameterByIndex(2).ParameterType.IsGenericParameter &&
					method.GetParameterByIndex(3).ParameterType.FullName == typeof(bool).FullName));
		}
	}
}