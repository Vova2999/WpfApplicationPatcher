using System;
using System.Linq;
using Castle.Components.DictionaryAdapter.Xml;
using log4net;
using Mono.Cecil;
using Mono.Cecil.Cil;
using WpfApplicationPatcher.Extensions;
using WpfApplicationPatcher.Helpers;

namespace WpfApplicationPatcher.Patchers.ViewModelPatchers {
	public class ViewModelPropertiesPatcher : IViewModelPatcher {
		private readonly ILog log;

		public ViewModelPropertiesPatcher() {
			log = Log.For(this);
		}

		public void Patch(ModuleDefinition module, TypeDefinition viewModelBaseType, TypeDefinition viewModelType) {
			log.Info($"Patching {viewModelType.FullName} properties...");

			var firstOrDefault = viewModelType.CustomAttributes.FirstOrDefault(attribute => attribute.Is(TypeNames.PatchingViewModelAttribute)).AttributeType.Resolve();
			

			//var customAttributeNamedArgument = firstOrDefault.Fields.FirstOrDefault(f => f.Name == "ViewModelPatchingType");
			//var argumentValue = customAttributeNamedArgument.Argument.Value;

			var properties = viewModelType.Properties
				.Select(property => new { Property = property, PropertyType = property.PropertyType.Resolve() })
				.Where(x =>
					x.Property.CustomAttributes.NotContains(TypeNames.NotPatchingPropertyAttribute) && (
						x.PropertyType.CustomAttributes.Contains(TypeNames.PatchingPropertyAttribute) ||
						x.PropertyType.IsNot(TypeNames.ICommand) && x.PropertyType.Interfaces.NotContains(TypeNames.ICommand)))
				.Select(x => x.Property)
				.ToArray();
			log.Debug($"Properties found:\r\n{string.Join("\r\n", properties.Select((property, index) => $"\t{index + 1}) {property.FullName}"))}");

			foreach (var property in properties) {
				log.Info($"Patching {property.FullName}...");

				var propertyName = property.Name;
				log.Debug($"Property name: {propertyName}");

				var firstCharacterOfPropertyName = propertyName.First();
				if (char.IsLower(firstCharacterOfPropertyName)) {
					log.Error("First character of property name must be to upper case");
					throw new Exception("Internal error of property patching");
				}

				var backgroundFieldName = $"{char.ToLower(firstCharacterOfPropertyName)}{propertyName.Substring(1)}";
				log.Debug($"Background field name: {backgroundFieldName}");

				var backgroundField = viewModelType.Fields.FirstOrDefault(field => field.Name == backgroundFieldName);

				if (backgroundField == null) {
					backgroundField = new FieldDefinition(backgroundFieldName, FieldAttributes.Private, property.PropertyType);
					viewModelType.Fields.Add(backgroundField);
					log.Debug("Background field was created");
				}
				else
					log.Debug("Background field was connected");

				log.Debug("Generate get method body...");
				var getMethodBodyInstructions = property.GetMethod.Body.Instructions;
				getMethodBodyInstructions.Clear();
				getMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldarg_0));
				getMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ldfld, backgroundField));
				getMethodBodyInstructions.Add(Instruction.Create(OpCodes.Ret));
				log.Debug("Get method body was generated");

				log.Debug("Generate method reference on Set method in ViewModelBase...");
				var methodDefinition = new GenericInstanceMethod(GetSetMethodInViewModelBase(viewModelBaseType));
				methodDefinition.GenericArguments.Add(property.PropertyType);
				var setMethodInViewModelBaseWithGenericParameter = module.Import(methodDefinition);
				log.Debug("Method reference was generated");

				log.Debug("Generate set method body...");
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
				log.Debug("Set method body was generated");
			}
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