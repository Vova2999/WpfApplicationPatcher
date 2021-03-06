﻿namespace WpfApplicationPatcher.Core.Types.Base {
	public abstract class ParameterBase<TParameter, TType> : ObjectBase<TParameter> where TParameter : class {
		public abstract string Name { get; }
		public abstract TType ParameterType { get; }

		protected ParameterBase(TParameter instance) : base(instance) {
		}
	}
}