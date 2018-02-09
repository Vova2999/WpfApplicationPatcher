﻿using System.Collections.Generic;
using Mono.Cecil;
using WpfApplicationPatcher.Core.Extensions;

namespace WpfApplicationPatcher.Core.Types.MonoCecil {
	public class MonoCecilModule : ObjectBase<ModuleDefinition> {
		public IEnumerable<MonoCecilType> Types => GetOrCreate(() => Instance.Types.ToMonoCecilTypes());

		internal MonoCecilModule(ModuleDefinition instance) : base(instance) {
		}

		public MonoCecilMethodReference Import(MonoCecilGenericInstanceMethod monoCecilGenericInstanceMethod) {
			return Instance.Import(monoCecilGenericInstanceMethod.Instance).ToMonoCecilMethodReference();
		}
	}
}