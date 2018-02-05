using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using WpfApplicationPatcher.AssemblyTypes;

namespace WpfApplicationPatcher.Extensions {
	public static class AssemblyTypeExtensions {
		public static IEnumerable<AssemblyType> WhereFrom(this IEnumerable<AssemblyType> assemblyTypes, ModuleDefinition module) {
			return assemblyTypes.Where(assemblyType => assemblyType.MonoCecilType.Module == module);
		}

		public static TAttribute GetReflectionAttribute<TAttribute>(this AssemblyType assemblyType) where TAttribute : Attribute {
			return assemblyType.Load().GetAttribute<TAttribute>()?.ReflectionAttribute as TAttribute;
		}

		public static AssemblyAttributeType GetAttribute<TAttribute>(this AssemblyType assemblyType) where TAttribute : Attribute {
			return assemblyType.Load().Attributes.FirstOrDefault(attribute => attribute.ReflectionAttribute.GetType() == typeof(TAttribute));
		}
	}
}