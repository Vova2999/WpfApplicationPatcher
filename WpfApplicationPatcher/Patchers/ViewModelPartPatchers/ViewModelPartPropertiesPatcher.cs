using System;
using System.Linq;
using System.Windows.Input;
using Mono.Cecil;
using Mono.Cecil.Cil;
using WpfApplicationPatcher.AssemblyTypes;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;
using WpfApplicationPatcher.Types.Attributes.Properties;
using WpfApplicationPatcher.Types.Enums;

namespace WpfApplicationPatcher.Patchers.ViewModelPartPatchers {
	public class ViewModelPartPropertiesPatcher : IViewModelPartPatcher {
		private readonly Log log;

		public ViewModelPartPropertiesPatcher() {
			log = Log.For(this);
		}

		[DoNotAddLogOffset]
		public void Patch(AssemblyDefinition monoCecilAssembly, AssemblyType viewModelBaseAssemblyType, AssemblyType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType) {
			log.Info($"Patching {viewModelAssemblyType.FullName} properties...");

			var assemblyPropertyTypes = GetViewModelProperties(viewModelAssemblyType, viewModelPatchingType);
			log.Debug("Properties found:", assemblyPropertyTypes.Select(property => property.FullName));

			foreach (var assemblyPropertyType in assemblyPropertyTypes) {
				log.Info($"Patching {assemblyPropertyType.FullName}...");
				PatchProprty(monoCecilAssembly, viewModelBaseAssemblyType, viewModelAssemblyType, assemblyPropertyType);
				log.Info($"{assemblyPropertyType.FullName} was patched");
			}

			log.Info($"Properties {viewModelAssemblyType.FullName} was patched");
		}

		[DoNotAddLogOffset]
		private AssemblyPropertyType[] GetViewModelProperties(AssemblyType viewModelAssemblyType, ViewModelPatchingType viewModelPatchingType) {
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

		private void PatchProprty(AssemblyDefinition monoCecilAssembly, AssemblyType viewModelBaseAssemblyType, AssemblyType viewModelAssemblyType, AssemblyPropertyType assemblyPropertyType) {
			CheckProperty(assemblyPropertyType);

			var propertyName = assemblyPropertyType.MonoCecilProperty.Name;
			log.Debug($"Property name: {propertyName}");

			var backgroundFieldName = $"{char.ToLower(propertyName.First())}{propertyName.Substring(1)}";
			log.Debug($"Background field name: {backgroundFieldName}");

			var backgroundField = viewModelAssemblyType.MonoCecilType.Fields.FirstOrDefault(field => field.Name == backgroundFieldName);

			if (backgroundField == null) {
				backgroundField = new FieldDefinition(backgroundFieldName, FieldAttributes.Private, assemblyPropertyType.MonoCecilProperty.PropertyType);
				viewModelAssemblyType.MonoCecilType.Fields.Add(backgroundField);
				log.Debug("Background field was created");
			}
			else
				log.Debug("Background field was connected");

			GenerateGetMethodBody(assemblyPropertyType.MonoCecilProperty, backgroundField);
			GenerateSetMethodBody(monoCecilAssembly, viewModelBaseAssemblyType, assemblyPropertyType.MonoCecilProperty, propertyName, backgroundField);
		}

		[DoNotAddLogOffset]
		private void CheckProperty(AssemblyPropertyType assemblyPropertyType) {
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
		private void GenerateGetMethodBody(PropertyDefinition property, FieldDefinition backgroundField) {
			log.Info("Generate get method body...");
			var getMethodBodyInstructions = property.GetMethod.Body.Instructions;
			getMethodBodyInstructions.Clear();
			getMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldarg_0));
			getMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldfld, backgroundField));
			getMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ret));
			log.Info("Get method body was generated");
		}

		[DoNotAddLogOffset]
		private void GenerateSetMethodBody(AssemblyDefinition monoCecilAssembly, AssemblyType viewModelBaseAssemblyType, PropertyDefinition property, string propertyName, FieldDefinition backgroundField) {
			log.Info("Generate method reference on Set method in ViewModelBase...");
			var methodDefinition = new GenericInstanceMethod(GetSetMethodInViewModelBase(viewModelBaseAssemblyType.MonoCecilType));
			methodDefinition.GenericArguments.Add(property.PropertyType);
			var setMethodInViewModelBaseWithGenericParameter = monoCecilAssembly.MainModule.Import(methodDefinition);
			log.Info("Method reference was generated");

			log.Info("Generate set method body...");
			var setMethodBodyInstructions = property.SetMethod.Body.Instructions;
			setMethodBodyInstructions.Clear();
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldarg_0));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldstr, propertyName));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldarg_0));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldflda, backgroundField));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldarg_1));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Call, setMethodInViewModelBaseWithGenericParameter));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Pop));
			setMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ret));
			log.Info("Set method body was generated");
		}

		private static MethodReference setMethodInViewModelBase;
		private static MethodReference GetSetMethodInViewModelBase(TypeDefinition viewModelBaseType) {
			return setMethodInViewModelBase ?? (setMethodInViewModelBase =
				viewModelBaseType.Methods.Single(method =>
					method.Name == "Set" &&
					method.Parameters.Count == 4 &&
					method.Parameters[0].ParameterType.FullName == typeof(string).FullName &&
					method.Parameters[1].ParameterType.IsByReference &&
					method.Parameters[2].ParameterType.IsGenericParameter &&
					method.Parameters[3].ParameterType.FullName == typeof(bool).FullName));
		}
	}
}